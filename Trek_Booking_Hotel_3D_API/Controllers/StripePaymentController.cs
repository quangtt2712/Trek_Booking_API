using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Hotel_3D_API.Helper;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StripePaymentController : ControllerBase
    {
        private readonly ILogger<StripePaymentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly StripeSettings _stripeSettings;
        private readonly IEmailSender _emailSender;


        public StripePaymentController(IEmailSender emailSender, IOptions<StripeSettings> stripeSettings, ILogger<StripePaymentController> logger, IConfiguration configuration,ApplicationDBContext context,IOrderRepository orderRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _orderRepository = orderRepository;
            _stripeSettings = stripeSettings.Value;
            _emailSender = emailSender;

        }

        [HttpPost("/StripePayment/Create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDTO paymentDTO)
        {
            try
            {
                // Log dữ liệu nhận được để kiểm tra
                Console.WriteLine(JsonConvert.SerializeObject(paymentDTO));

                var domain = _configuration.GetValue<string>("Trekbooking_Client_URL");

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + paymentDTO.SuccessUrl,
                    CancelUrl = domain + paymentDTO.CancelUrl,
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                // Sử dụng OrderHeader để lấy tổng giá
                var totalPrice = paymentDTO.Order.OrderHeader.TotalPrice;

                // Tạo một chuỗi kết hợp tên phòng và tên khách sạn từ OrderDetails
                var roomHotelNames = paymentDTO.Order.OrderDetails.Select(d => $"Room Name: {d.RoomName} at hotel {d.HotelName}").ToList();
                var combinedNames = string.Join(", ", roomHotelNames);

                var sessionLineItem = new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(totalPrice * 100), // Tổng giá đã tính sẵn
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = combinedNames // Tên phòng và tên khách sạn kết hợp
                        }
                    },
                    Quantity = 1 // Số lượng luôn là 1 vì tổng giá đã bao gồm số đêm
                };
                options.LineItems.Add(sessionLineItem);

                var service = new Stripe.Checkout.SessionService();
                var session = await service.CreateAsync(options);
                paymentDTO.Order.OrderHeader.SessionId = session.Id;
                paymentDTO.Order.OrderHeader.PaymentIntentId = session.PaymentIntentId;
                var createdOrder = await _orderRepository.Create(paymentDTO.Order);
                return Ok(new SuccessModelDTO
                {
                    Data = session.Id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new ErrorModelDTO
                {
                    ErrorMessage = ex.Message,
                });
            }
        }


        [HttpPost("/StripePayment/Confirm")]
        public async Task<IActionResult> Confirm()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];

            if (string.IsNullOrEmpty(signatureHeader))
            {
                return BadRequest("Missing Stripe-Signature header.");
            }

            var webhookSecret = _configuration.GetValue<string>("Stripe:WebhookSecret");
            if (string.IsNullOrEmpty(webhookSecret))
            {
                throw new InvalidOperationException("Stripe webhook secret is not configured.");
            }

            Stripe.Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, webhookSecret, throwOnApiVersionMismatch: false);
            }
            catch (StripeException e)
            {
                _logger.LogError($"Unable to construct Stripe event: {e.Message}");
                return BadRequest($"Unable to construct Stripe event: {e.Message}");
            }

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                if (session == null)
                {
                    _logger.LogError("Session is null.");
                    return BadRequest("Session is null.");
                }

                // Tìm đơn hàng bằng sessionId
                var order = await _orderRepository.GetOrderBySessionId(session.Id);
                if (order != null)
                {
                 // mail
                    string emailContent = "Cảm ơn anh/chị đã booking tại web https://trek-booking.vercel.app  ";
                    await _emailSender.SendEmailAsync(order.Email, "TrekBooking ", emailContent);

                    order.PaymentIntentId = session.PaymentIntentId;
                    order.Process = "Paid"; // Cập nhật trạng thái thành công
                    await _orderRepository.Update(order);

                    
               
                

                    // Tải chi tiết đơn hàng
                    var orderDetails = await _context.OrderHotelDetails
                        .Where(od => od.OrderHotelHeaderlId == order.Id)
                        .ToListAsync();

                    // Xóa cart
                    foreach (var detail in orderDetails)
                    {
                        await ClearCart(detail.RoomId);
                    }

                    return Ok();
                }
                else
                {
                    _logger.LogError("Order not found for session id: " + session.Id);
                    return NotFound("Order not found");
                }
            }

            return Ok();
        }



        private async Task ClearCart(int? roomId)
        {
            try
            {
                var cartItem = _context.bookingCarts.FirstOrDefault(ci => ci.RoomId == roomId);
                if (cartItem != null)
                {
                    _context.bookingCarts.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cart cleared successfully for roomId: {roomId}", roomId);
                }
                else
                {
                    _logger.LogWarning("Cart item not found for roomId: {roomId}", roomId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error clearing cart: {message}", ex.Message);
            }
        }

        [HttpPost("/StripePayment/CreateTour")]
        public async Task<IActionResult> CreateTour([FromBody] StripePaymentTourDTO paymentDTO)
        {
            try
            {
                // Log dữ liệu nhận được để kiểm tra
                Console.WriteLine(JsonConvert.SerializeObject(paymentDTO));

                var domain = _configuration.GetValue<string>("Trekbooking_Client_URL");

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + paymentDTO.SuccessUrl,
                    CancelUrl = domain + paymentDTO.CancelUrl,
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                // Sử dụng OrderHeader để lấy tổng giá
                var totalPrice = paymentDTO.Order.OrderHeader.TotalPrice;


                var TourNames = paymentDTO.Order.OrderDetails.Select(d => $"Tour Name: {d.TourName}").ToList();
                var combinedNames = string.Join(", ", TourNames);

                var sessionLineItem = new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(totalPrice * 100), // Tổng giá đã tính sẵn
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = combinedNames // Tên phòng và tên khách sạn kết hợp
                        }
                    },
                    Quantity = 1
                };
                options.LineItems.Add(sessionLineItem);

                var service = new Stripe.Checkout.SessionService();
                var session = await service.CreateAsync(options);

                paymentDTO.Order.OrderHeader.SessionId = session.Id;
                paymentDTO.Order.OrderHeader.PaymentIntentId = session.PaymentIntentId;


                var createdOrder = await _orderRepository.CreateTour(paymentDTO.Order);
          

                return Ok(new SuccessModelDTO
                {
                    Data = session.Id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new ErrorModelDTO
                {
                    ErrorMessage = ex.Message,
                });
            }
        }
        [HttpPost("/StripePayment/ConfirmTour")]
        public async Task<IActionResult> ConfirmTour()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(signatureHeader))
            {
                _logger.LogError("Missing Stripe-Signature header.");
                return BadRequest("Missing Stripe-Signature header.");
            }

            var webhookSecretTour = _configuration.GetValue<string>("Stripe:WebhookSecretTour");
            if (string.IsNullOrEmpty(webhookSecretTour))
            {
                _logger.LogError("Stripe webhook secret for tour is not configured.");
                throw new InvalidOperationException("Stripe webhook secret for tour is not configured.");
            }

            Stripe.Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, webhookSecretTour, throwOnApiVersionMismatch: false);
            }
            catch (StripeException e)
            {
                _logger.LogError($"Unable to construct Stripe event: {e.Message}");
                return BadRequest($"Unable to construct Stripe event: {e.Message}");
            }

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                if (session == null)
                {
                    _logger.LogError("Session is null.");
                    return BadRequest("Session is null.");
                }

                _logger.LogInformation($"Session ID: {session.Id}, Payment Intent ID: {session.PaymentIntentId}, Email: {session.CustomerDetails.Email}");

                var order = await _orderRepository.GetOrderTourBySessionId(session.Id);
                if (order != null)
                {
                    //mail tour
                    // mail
                    string emailContent = "Cảm ơn anh/chị đã booking tại web https://trek-booking.vercel.app  ";
                    await _emailSender.SendEmailAsync(order.Email, "TrekBooking ", emailContent);


                    order.PaymentIntentId = session.PaymentIntentId;
                    order.Process = "Paid";
                    await _orderRepository.UpdateTour(order);

                    var orderDetails = await _context.OrderTourDetails
                        .Where(od => od.OrderTourHeaderlId == order.Id)
                        .ToListAsync();

                    foreach (var detail in orderDetails)
                    {
                        await ClearCartTour(detail.TourId);
                    }

                    return Ok();
                }
                else
                {
                    _logger.LogError($"Order not found for session ID: {session.Id}");
                    return NotFound("Order not found");
                }
            }

            return Ok();
        }


        private async Task ClearCartTour(int tourId)
        {
            try
            {
                var cartItem = _context.cartTours.FirstOrDefault(ct => ct.TourId == tourId);
                if (cartItem != null)
                {
                    _context.cartTours.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("CartTour cleared successfully for tourId: {tourId}", tourId);
                }
                else
                {
                    _logger.LogWarning("CartTour item not found for tourId: {tourId}", tourId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error clearing CartTour: {message}", ex.Message);
            }
        }



        //[HttpDelete("{roomId}")]
        //public async Task<IActionResult> ClearCart(int? roomId)
        //{
        //    try
        //    {
        //        var cartItem = _context.bookingCarts.FirstOrDefault(ci => ci.RoomId == roomId);
        //        if (cartItem != null)
        //        {
        //            _context.bookingCarts.Remove(cartItem);
        //            await _context.SaveChangesAsync();
        //            _logger.LogInformation("Cart cleared successfully for roomId: {roomId}", roomId);
        //            return Ok(new { message = "Cart cleared successfully" });
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Cart item not found for roomId: {roomId}", roomId);
        //            return NotFound(new { message = "Cart item not found" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error clearing cart: {message}", ex.Message);
        //        return StatusCode(500, new { message = "Error clearing cart", error = ex.Message });
        //    }
        //}




    }
}
