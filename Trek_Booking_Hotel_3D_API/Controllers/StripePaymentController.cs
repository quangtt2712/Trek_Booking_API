using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
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

        public StripePaymentController(ILogger<StripePaymentController> logger, IConfiguration configuration,ApplicationDBContext context,IOrderRepository orderRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _orderRepository = orderRepository;
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

                // Gọi hàm lưu đơn hàng sau khi thanh toán thành công
                paymentDTO.Order.OrderHeader.SessionId = session.Id;
                paymentDTO.Order.OrderHeader.PaymentIntentId = session.PaymentIntentId;

                // Giả sử bạn muốn lưu trường Requirement từ phía client
                // paymentDTO.Order.OrderHeader.Requirement = paymentDTO.Order.OrderHeader.Requirement;

                var createdOrder = await _orderRepository.Create(paymentDTO.Order);
                foreach (var detail in paymentDTO.Order.OrderDetails)
                {
                   await ClearCart(detail.RoomId);
                }
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



        [HttpDelete("{roomId}")]
        public async Task<IActionResult> ClearCart(int? roomId)
        {
            try
            {
                var cartItem = _context.bookingCarts.FirstOrDefault(ci => ci.RoomId == roomId);
                if (cartItem != null)
                {
                    _context.bookingCarts.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cart cleared successfully for roomId: {roomId}", roomId);
                    return Ok(new { message = "Cart cleared successfully" });
                }
                else
                {
                    _logger.LogWarning("Cart item not found for roomId: {roomId}", roomId);
                    return NotFound(new { message = "Cart item not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error clearing cart: {message}", ex.Message);
                return StatusCode(500, new { message = "Error clearing cart", error = ex.Message });
            }
        }
        //private async Task ClearCart(int? roomId)
        //{
        //    try
        //    {
        //        var cartItem = _context.bookingCarts.FirstOrDefault(ci => ci.RoomId == roomId);
        //        if (cartItem != null)
        //        {
        //            _context.bookingCarts.Remove(cartItem);
        //            await _context.SaveChangesAsync();
        //            _logger.LogInformation("Cart cleared successfully for roomId: {roomId}", roomId);
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Cart item not found for roomId: {roomId}", roomId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error clearing cart: {message}", ex.Message);
        //    }
        //}

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

                // Gọi hàm lưu đơn hàng sau khi thanh toán thành công
                paymentDTO.Order.OrderHeader.SessionId = session.Id;
                paymentDTO.Order.OrderHeader.PaymentIntentId = session.PaymentIntentId;

                // Giả sử bạn muốn lưu trường Requirement từ phía client
                // paymentDTO.Order.OrderHeader.Requirement = paymentDTO.Order.OrderHeader.Requirement;

                var createdOrder = await _orderRepository.CreateTour(paymentDTO.Order);
                foreach (var detail in paymentDTO.Order.OrderDetails)
                {
                    await ClearCartTour(detail.TourId);
                }

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


    }
}
