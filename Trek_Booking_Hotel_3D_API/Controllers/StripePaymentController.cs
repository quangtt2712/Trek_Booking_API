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
using Trek_Booking_Repository.Repositories;
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
        private readonly IOrderHotelDetailRepository _orderHotelDetailRepository;
        private readonly StripeSettings _stripeSettings;
        private readonly IEmailSender _emailSender;
        private readonly IVoucherUsageHistoryRepository _repository;
        private readonly IPaymentInforRepository _repositoryin;


        public StripePaymentController(IPaymentInforRepository repositoryin, IVoucherUsageHistoryRepository repository, IEmailSender emailSender, IOptions<StripeSettings> stripeSettings, ILogger<StripePaymentController> logger, IConfiguration configuration,ApplicationDBContext context,IOrderRepository orderRepository, IOrderHotelDetailRepository orderHotelDetailRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _orderRepository = orderRepository;
            _stripeSettings = stripeSettings.Value;
            _emailSender = emailSender;
            _repository = repository;
            _repositoryin = repositoryin;
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
                    var paymentInfor = new PaymentInformation
                    {
                        PaymentInforId = 0, // Tự động tăng
                        PaymentMethod = "Visa",
                        CartNumber = "4242 4242 4242 4242",
                        TotalPrice = order.TotalPrice,
                        PaymentFee = 0,
                        PaidDate = DateTime.UtcNow,
                        UserId = order.UserId,
                        
                    };

                    await _repositoryin.createPaymentInfor(paymentInfor);


                    string emailContent = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {{ font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4; }}
                            .container {{ width: 80%; margin: auto; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                            .header {{ background: #4CAF50; color: white; padding: 10px 0; text-align: center; }}
                            .content {{ margin: 20px 0; }}
                            .content p {{ line-height: 1.6; }}
                            .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
                            .footer a {{ color: #4CAF50; text-decoration: none; }}
                            .button {{ display: inline-block; padding: 10px 20px; font-size: 16px; background: #4CAF50; color: white; text-decoration: none; border-radius: 5px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Thank You for Your Booking!</h1>
                            </div>
                            <div class='content'>
                                <p>Dear {order.FullName},</p>

                    <p>Check in date: {order.CheckInDate}</p>
                    <p>Check out date: {order.CheckOutDate}</p>
                    <h4>Total price: {order.TotalPrice} $</4>
                                <p>Thank you for booking with us at <a href='https://trek-booking.vercel.app'>Trek Booking</a>. We are thrilled to have you as our guest.</p>
                                <p>For more details, please visit our website.</p>
                                <p>If you have any questions, do not hesitate to contact us.</p>
                                <p>We wish you a pleasant and memorable journey!</p>
                                <p>Best regards,</p>
                                <p>The Trek Booking Team</p>
                                <a href='https://trek-booking.vercel.app' class='button'>Visit Trek Booking</a>
                            </div>
                            <div class='footer'>
                                <p>&copy; 2024 Trek Booking. All rights reserved.</p>
                                <p><a href='https://trek-booking.vercel.app'>Visit our website</a></p>
                            </div>
                        </div>
                    </body>
                    </html>
                    ";

                    // mail

                    // Send the email
                    await _emailSender.SendEmailAsync(order.Email, "TrekBooking - Thank You for Your Booking!", emailContent);

                    // Update order information
                    //string emailContent = "Cảm ơn anh/chị đã booking tại web https://trek-booking.vercel.app  ";
                    //await _emailSender.SendEmailAsync(order.Email, "TrekBooking ", emailContent);


                    order.PaymentIntentId = session.PaymentIntentId;
                    order.Process = "Paid"; // Update status to successful
                    await _orderRepository.Update(order);
                    if (order.Id != 0)
                    {
                        var voucherUsageHistory = new VoucherUsageHistory
                        {
                            UserVoucherId = 0,
                            VoucherId = order.VoucherId, // Giả sử bạn có thông tin này trong order
                            UserId = order.UserId,
                            OrderHotelHeaderId = order.Id // Sử dụng OrderHotelHeaderId mới
                        };

                        await _repository.createVoucherUsageHistory(voucherUsageHistory);
                        var voucher = await _context.vouchers.FindAsync(order.VoucherId);
                        if (voucher != null)
                        {
                            voucher.VoucherQuantity -= 1; // Giảm số lượng voucher
                            _context.vouchers.Update(voucher);
                            await _context.SaveChangesAsync();
                        }
                    }
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
                    string emailContent = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4; }}
        .container {{ width: 80%; margin: auto; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
        .header {{ background: #4CAF50; color: white; padding: 10px 0; text-align: center; }}
        .content {{ margin: 20px 0; }}
        .content p {{ line-height: 1.6; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #777; }}
        .footer a {{ color: #4CAF50; text-decoration: none; }}
        .button {{ display: inline-block; padding: 10px 20px; font-size: 16px; background: #4CAF50; color: white; text-decoration: none; border-radius: 5px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Thank You for Your Booking!</h1>
        </div>
        <div class='content'>
            <p>Dear {order.FullName},</p>
<p>You have successfully booked the tour!</p>
<p>Tour order date: {order.TourOrderDate}</p>
<h4>Total price: {order.TotalPrice} $</4>
            <p>Thank you for booking with us at <a href='https://trek-booking.vercel.app'>Trek Booking</a>. We are thrilled to have you as our guest.</p>
            <p>For more details, please visit our website.</p>
            <p>If you have any questions, do not hesitate to contact us.</p>
            <p>We wish you a pleasant and memorable journey!</p>
            <p>Best regards,</p>
            <p>The Trek Booking Team</p>
            <a href='https://trek-booking.vercel.app' class='button'>Visit Trek Booking</a>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Trek Booking. All rights reserved.</p>
            <p><a href='https://trek-booking.vercel.app'>Visit our website</a></p>
        </div>
    </div>
</body>
</html>
";
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
