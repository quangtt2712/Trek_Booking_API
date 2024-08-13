using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class OrderHotelDetailRepository : IOrderHotelDetailRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderHotelDetailRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<RoomDateRange>> getMostFrequentlyRoomBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate)
        {
            var result = await (from detail in _dbContext.OrderHotelDetails
                                join header in _dbContext.OrderHotelHeaders
                                on detail.OrderHotelHeaderlId equals header.Id
                                join hotel in _dbContext.hotels
                                on detail.HotelId equals hotel.HotelId
                                where header.CheckOutDate >= startDate && header.CheckOutDate <= endDate
                                && hotel.SupplierId == supplierId && header.Process == "Success"
                                && header.Completed == true
                                group detail by new { detail.RoomName, detail.HotelName } into g
                                select new RoomDateRange
                                {
                                    RoomName = g.Key.RoomName,
                                    HotelName = g.Key.HotelName,
                                    OrderCount = g.Count()
                                })
                           .OrderByDescending(o => o.OrderCount)
                           //.Take(5)
                           .ToListAsync();
            return result;
        }

        public async Task<OrderHotelDetail> getOrderHotelDetailByOrderHotelHeaderId(int orderHotelHeaderId)
        {
            var check = await _dbContext.OrderHotelDetails.FirstOrDefaultAsync(t => t.OrderHotelHeaderlId == orderHotelHeaderId);
            return check;
        }

        public async Task<IEnumerable<TopRoom>> getTop5RoomInWeek(int supplierId, DateTime startOfWeek, DateTime endOfWeek)
        {
            var result = await (from detail in _dbContext.OrderHotelDetails
                                join header in _dbContext.OrderHotelHeaders
                                on detail.OrderHotelHeaderlId equals header.Id
                                join hotel in _dbContext.hotels
                                on detail.HotelId equals hotel.HotelId
                                where header.CheckOutDate >= startOfWeek && header.CheckOutDate <= endOfWeek
                                && hotel.SupplierId == supplierId && header.Process == "Success"
                                && header.Completed == true
                                group detail by new { detail.RoomId, detail.RoomName, detail.HotelName } into g
                                select new TopRoom
                                {
                                    RoomId = g.Key.RoomId.Value,
                                    RoomName = g.Key.RoomName,
                                    HotelName = g.Key.HotelName,
                                    OrderCount = g.Count()
                                })
                           .OrderByDescending(o => o.OrderCount)
                           // .Take(5)
                           .ToListAsync();
            return result;
        }


        public async Task<IEnumerable<Top5Room>> getTop5RoomOrders(int supplierId)
        {
            var topRooms = await _dbContext.OrderHotelDetails
            .Where(o => o.Hotel.SupplierId == supplierId)
            .GroupBy(o => new { o.RoomName, o.Hotel.HotelName })
            .Select(g => new Top5Room
            {
                HotelName = g.Key.HotelName,
                Name = g.Key.RoomName,
                OrderCount = g.Count()
            })
            .OrderByDescending(t => t.OrderCount)
            .Take(5)
            .ToListAsync();

            return topRooms;
        }
    }
}
