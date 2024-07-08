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
    public class OrderHotelHeaderRepository : IOrderHotelHeaderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderHotelHeaderRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderByUserId(int userId)
        {
            var check = await _dbContext.OrderHotelHeaders.Where(u => u.UserId == userId && u.Process == "Success").ToListAsync();
            return check;
        }
        public async Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderBySupplierId(int supplierId)
        {
            var check = await _dbContext.OrderHotelHeaders.Where(u => u.SupplierId == supplierId && u.Process== "Success").ToListAsync();
            return check;
        }
        public async Task<int> countTotalOrderHotelBySupplierId(int supplierId)
        {
            var count = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success").CountAsync();
            return count;
        }
        public async Task<double> getPercentChangeFromLastWeek(int supplierId, DateTime date)
        {
            // Get the current week's count
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekCount = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.CheckInDate >= startOfCurrentWeek && s.CheckInDate < endOfCurrentWeek)
                .CountAsync();

            // Get the previous week's count
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekCount = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.CheckInDate >= startOfPreviousWeek && s.CheckInDate < endOfPreviousWeek)
                .CountAsync();

            // Calculate the percentage change
            if (previousWeekCount == 0) return 0;

            var percentageChange = ((currentWeekCount - previousWeekCount) / (double)previousWeekCount) * 100;
            return percentageChange;
        }

        public async Task<decimal> getPercentChangeRevenueFromLastWeek(int supplierId, DateTime date)
        {
            // Get the current week's revenue
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekRevenue = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.CheckInDate >= startOfCurrentWeek && s.CheckInDate < endOfCurrentWeek)
                .SumAsync(t => t.TotalPrice ?? 0);

            // Get the previous week's revenue
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekRevenue = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.CheckInDate >= startOfPreviousWeek && s.CheckInDate < endOfPreviousWeek)
                .SumAsync(t => t.TotalPrice ?? 0);

            // Calculate the percentage change
            if (previousWeekRevenue == 0) return 0;

            var percentageChange = ((currentWeekRevenue - previousWeekRevenue) / previousWeekRevenue) * 100;
            return percentageChange;
        }

        public async Task<IEnumerable<AnnualRevenue>> getRevenueYearBySupplierId(int supplierId)
        {
            var annualRevenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId && o.Process == "Success")
                .GroupBy(o => o.CheckInDate.Value.Year)
                .Select(g => new AnnualRevenue
                {
                    Year = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
                .OrderBy(x => x.Year)
                .ToListAsync();

            return annualRevenue;
        }

        public async Task<decimal> getTotalRevenueHotelBySupplierId(int supplierId)
        {
            var totalRevenue = await _dbContext.OrderHotelHeaders
                .Where(u => u.SupplierId == supplierId && u.Process == "Success")
       .SumAsync(t => t.TotalPrice ?? 0);
            return totalRevenue;
        }


    }
}
