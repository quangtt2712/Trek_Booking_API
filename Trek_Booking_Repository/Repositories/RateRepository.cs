using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Trek_Booking_Repository.Repositories
{
    public class RateRepository : IRateRepository
    {
        private readonly ApplicationDBContext _context;

        public RateRepository(ApplicationDBContext context)
        {
            _context = context;
        }




        public async Task<Rate> rateHotel(Rate rate)
        {
            var bookingId = rate.BookingId;
            var orderHotelHeaderId = rate.OrderHotelHeaderId;
            var userId = rate.UserId;
            var hotelId = rate.HotelId;
            // Kiểm tra xem người dùng đã đặt phòng chưa
            var checkBooked = await _context.OrderHotelHeaders.FirstOrDefaultAsync(b => b.Id == orderHotelHeaderId && b.UserId == userId);
            if (checkBooked == null)
            {
                // Nếu người dùng chưa đặt phòng, bạn có thể throw một Exception hoặc xử lý theo ý của bạn
                throw new Exception("User has not booked this room.");
            }

            // Người dùng đã đặt phòng, tiến hành tạo bình luận
            var newRate = new Rate
            {
                BookingId = rate.BookingId,
                OrderHotelHeaderId = rate.OrderHotelHeaderId,
                RateValue = rate.RateValue,
                HotelId = rate.HotelId,
                UserId = rate.UserId
            };
            _context.rates.Add(newRate);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<IEnumerable<Rate>> getRateByHotelId(int hotelId)
        {

            var rates = await _context.rates.Include(t => t.User).Include(t => t.Booking).Include(t => t.Hotel).Where(t => t.HotelId == hotelId).ToListAsync();

            return rates;
        }


        public async Task<IEnumerable<Rate>> getRateByUserId(int userId)
        {
            var rates = await _context.rates.Where(t => t.UserId == userId).ToListAsync();
            return rates;
        }

        public async Task<IEnumerable<Rate>> getRateByOrderHotelHeaderId(int OrderHotelHeaderId)
        {

            var rates = await _context.rates.Where(t => t.OrderHotelHeaderId == OrderHotelHeaderId).ToListAsync();

            return rates;
        }


        public async Task<float> getTotalRateValueByHotelId(int hotelId)
        {
            var rates = await _context.rates.Where(t => t.HotelId == hotelId).ToListAsync();
            if (rates.Count == 0)
            {
                return 0;
            }

            // Tính trung bình của trường RateValue
            float totalRateValue = (float)rates.Average(rate => rate.RateValue);
            return totalRateValue;
        }
    }
}
