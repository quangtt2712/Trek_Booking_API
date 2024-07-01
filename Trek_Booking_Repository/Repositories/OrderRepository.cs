using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
           
        }
        public async Task<OrderDTO> Create(OrderDTO objDTO)
        {
            try
            {
                // Tạo OrderHotelHeader từ objDTO.OrderHeader
                var orderHeader = new OrderHotelHeader
                {
                    UserId = objDTO.OrderHeader.UserId,
                    TotalPrice = objDTO.OrderHeader.TotalPrice,
                    CheckInDate = objDTO.OrderHeader.CheckInDate,
                    CheckOutDate = objDTO.OrderHeader.CheckOutDate,
                    SessionId = objDTO.OrderHeader.SessionId,
                    PaymentIntentId = objDTO.OrderHeader.PaymentIntentId,
                    FullName = objDTO.OrderHeader.FullName,
                    Email = objDTO.OrderHeader.Email,
                    Phone = objDTO.OrderHeader.Phone,
                    Requirement = objDTO.OrderHeader.Requirement,
                 
                };

                _context.OrderHotelHeaders.Add(orderHeader);
                await _context.SaveChangesAsync();

                var orderDetails = new List<OrderHotelDetail>();
                foreach (var detail in objDTO.OrderDetails)
                {
                    // Tạo OrderHotelDetail từ objDTO.OrderDetails
                    var orderDetail = new OrderHotelDetail
                    {
                        OrderHotelHeaderlId = orderHeader.Id,
                        HotelId = detail.HotelId,
                        RoomId = detail.RoomId,
                        Price = detail.Price,
                        RoomQuantity = detail.RoomQuantity,
                        RoomName = detail.RoomName,
                        HotelName = detail.HotelName,
                    };
                    orderDetails.Add(orderDetail);
                    _context.OrderHotelDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                // Chuẩn bị dữ liệu trả về
                var result = new OrderDTO
                {
                    OrderHeader = new OrderHotelHeader
                    {
                        Id = orderHeader.Id,
                        UserId = orderHeader.UserId,
                        TotalPrice = orderHeader.TotalPrice,
                        CheckInDate = orderHeader.CheckInDate,
                        CheckOutDate = orderHeader.CheckOutDate,
                        SessionId = orderHeader.SessionId,
                        PaymentIntentId = orderHeader.PaymentIntentId,
                        FullName = orderHeader.FullName,
                        Email = orderHeader.Email,
                        Phone = orderHeader.Phone,
                        Requirement = orderHeader.Requirement,
                       
                    },
                    OrderDetails = orderDetails.Select(d => new OrderHotelDetail
                    {
                        Id = d.Id,
                        OrderHotelHeaderlId = d.OrderHotelHeaderlId,
                        HotelId = d.HotelId,
                        RoomId = d.RoomId,
                        Price = d.Price,
                        RoomQuantity = d.RoomQuantity,
                        RoomName = d.RoomName
                    }).ToList()
                };

                return result;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                throw new Exception("An error occurred while creating the order.", ex);
            }
        }



       

    }
}
