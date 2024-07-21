using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<OrderTourHeader> GetOrderTourBySessionId(string sessionId)
        {
            return await _context.OrderTourHeaders
                 .FirstOrDefaultAsync(o => o.SessionId == sessionId);
        }

        public async Task UpdateTour(OrderTourHeader orderTour)
        {
            _context.OrderTourHeaders.Update(orderTour);
            await _context.SaveChangesAsync();
        }
        public async Task<OrderHotelHeader> GetOrderBySessionId(string sessionId)
        {
            return await _context.OrderHotelHeaders
                .FirstOrDefaultAsync(o => o.SessionId == sessionId);
        }

        public async Task Update(OrderHotelHeader order)
        {
            _context.OrderHotelHeaders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDTO> Create(OrderDTO objDTO)
        {
            try
            {
                // Tạo OrderHotelHeader từ objDTO.OrderHeader
                var orderHeader = new OrderHotelHeader
                {
                    UserId = objDTO.OrderHeader.UserId,
                    SupplierId = objDTO.OrderHeader.SupplierId,
                    TotalPrice = objDTO.OrderHeader.TotalPrice,
                    CheckInDate = objDTO.OrderHeader.CheckInDate,
                    CheckOutDate = objDTO.OrderHeader.CheckOutDate,
                    SessionId = objDTO.OrderHeader.SessionId,
                    PaymentIntentId = objDTO.OrderHeader.PaymentIntentId,
                    FullName = objDTO.OrderHeader.FullName,
                    Email = objDTO.OrderHeader.Email,
                    Phone = objDTO.OrderHeader.Phone,
                    Requirement = objDTO.OrderHeader.Requirement,
                    Process = objDTO.OrderHeader.Process,
                    Completed = objDTO.OrderHeader.Completed,
                    VoucherCode = objDTO.OrderHeader.VoucherCode,
                    VoucherId = objDTO.OrderHeader.VoucherId
                    
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
                        SupplierId = objDTO.OrderHeader.SupplierId,
                        TotalPrice = orderHeader.TotalPrice,
                        CheckInDate = orderHeader.CheckInDate,
                        CheckOutDate = orderHeader.CheckOutDate,
                        SessionId = orderHeader.SessionId,
                        PaymentIntentId = orderHeader.PaymentIntentId,
                        FullName = orderHeader.FullName,
                        Email = orderHeader.Email,
                        Phone = orderHeader.Phone,
                        Requirement = orderHeader.Requirement,
                       Process = orderHeader.Process,
                       Completed = orderHeader.Completed,
                       VoucherCode = orderHeader.VoucherCode,
                       VoucherId = orderHeader.VoucherId,
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

        public async Task<OrderTourDTO> CreateTour(OrderTourDTO objDTO)
        {
            try
            {
                // Tạo OrderHotelHeader từ objDTO.OrderHeader
                var orderHeader = new OrderTourHeader
                {
                    UserId = objDTO.OrderHeader.UserId,
                    SupplierId = objDTO.OrderHeader.SupplierId,
                    TotalPrice = objDTO.OrderHeader.TotalPrice,
                    TourOrderDate = objDTO.OrderHeader.TourOrderDate,
                    SessionId = objDTO.OrderHeader.SessionId,
                    PaymentIntentId = objDTO.OrderHeader.PaymentIntentId,
                    FullName = objDTO.OrderHeader.FullName,
                    Email = objDTO.OrderHeader.Email,
                    Phone = objDTO.OrderHeader.Phone,
                    Process = objDTO.OrderHeader.Process,
                    Completed = objDTO.OrderHeader.Completed,
                };

                _context.OrderTourHeaders.Add(orderHeader);
                await _context.SaveChangesAsync();

                var orderDetails = new List<OrderTourDetail>();
                foreach (var detail in objDTO.OrderDetails)
                {
                    // Tạo OrderHotelDetail từ objDTO.OrderDetails
                    var orderDetail = new OrderTourDetail
                    {
                        OrderTourHeaderlId = orderHeader.Id,
                        TourId = detail.TourId,
                        TourName = detail.TourName,
                        TourOrderQuantity = detail.TourOrderQuantity,
                        TourTotalPrice = detail.TourTotalPrice,
                    };
                    orderDetails.Add(orderDetail);
                    _context.OrderTourDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                // Chuẩn bị dữ liệu trả về
                var result = new OrderTourDTO
                {
                    OrderHeader = new OrderTourHeader
                    {
                        Id = orderHeader.Id,
                        UserId = orderHeader.UserId,
                        SupplierId = objDTO.OrderHeader.SupplierId,
                        TotalPrice = orderHeader.TotalPrice,
                        TourOrderDate = orderHeader.TourOrderDate,
                        SessionId = orderHeader.SessionId,
                        PaymentIntentId = orderHeader.PaymentIntentId,
                        FullName = orderHeader.FullName,
                        Email = orderHeader.Email,
                        Phone = orderHeader.Phone,
                        Process = orderHeader.Process,
                        Completed = orderHeader.Completed,
                    },
                    OrderDetails = orderDetails.Select(d => new OrderTourDetail
                    {
                        Id = d.Id,
                        OrderTourHeaderlId = d.OrderTourHeaderlId,
                        TourId = d.TourId,
                        TourName = d.TourName,
                        TourOrderQuantity = d.TourOrderQuantity,
                        TourTotalPrice = d.TourTotalPrice,
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
