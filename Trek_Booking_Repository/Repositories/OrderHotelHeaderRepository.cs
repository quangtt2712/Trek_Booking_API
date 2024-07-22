using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Trek_Booking_Repository.Repositories
{
    public class OrderHotelHeaderRepository : IOrderHotelHeaderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderHotelHeaderRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<OrderHotelHeader> updateOrderHotelHeader(OrderHotelHeader orderHotelHeader)
        {
            var findOrderHotelHeader = await _dbContext.OrderHotelHeaders.FirstOrDefaultAsync(b => b.Id == orderHotelHeader.Id);
            if (findOrderHotelHeader != null)
            {
                findOrderHotelHeader.Process = orderHotelHeader.Process;
                _dbContext.OrderHotelHeaders.Update(findOrderHotelHeader);
                await _dbContext.SaveChangesAsync();
                return findOrderHotelHeader;
            }
            return null;
        }
        public async Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderByUserId(int userId)
        {
            var check = await _dbContext.OrderHotelHeaders.Where(u => u.UserId == userId && (u.Process == "Paid" || u.Process == "Success")).ToListAsync();
            return check;
        }
        public async Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderBySupplierId(int supplierId)
        {
            var check = await _dbContext.OrderHotelHeaders.Where(u => u.SupplierId == supplierId && (u.Process== "Paid" || u.Process == "Success")).ToListAsync();
            return check;
        }
        public async Task<int> countTotalOrderHotelBySupplierId(int supplierId)
        {
            var count = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true).CountAsync();
            return count;
        }
        public async Task<double> getPercentChangeFromLastWeek(int supplierId, DateTime date)
        {
            // Get the current week's count
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekCount = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckInDate >= startOfCurrentWeek && s.CheckInDate < endOfCurrentWeek)
                .CountAsync();

            // Get the previous week's count
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekCount = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckInDate >= startOfPreviousWeek && s.CheckInDate < endOfPreviousWeek)
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
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckInDate >= startOfCurrentWeek && s.CheckInDate < endOfCurrentWeek)
                .SumAsync(t => t.TotalPrice ?? 0);

            // Get the previous week's revenue
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekRevenue = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckInDate >= startOfPreviousWeek && s.CheckInDate < endOfPreviousWeek)
                .SumAsync(t => t.TotalPrice ?? 0);

            // Calculate the percentage change
            if (previousWeekRevenue == 0) return 0;

            var percentageChange = ((currentWeekRevenue - previousWeekRevenue) / previousWeekRevenue) * 100;
            return percentageChange;
        }

        public async Task<IEnumerable<AnnualRevenue>> getRevenueYearBySupplierId(int supplierId)
        {
            var annualRevenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId && o.Process == "Success" && o.Completed == true)
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
                .Where(u => u.SupplierId == supplierId && u.Process == "Success" && u.Completed == true)
       .SumAsync(t => t.TotalPrice ?? 0);
            return totalRevenue;
        }

        public async Task<IEnumerable<WeeklyRevenue>> getCurrentWeekRevenueHotelBySupplierId(int supplierId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var weeklyRevenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId
                       && o.Process == "Success"
                       && o.Completed == true
                       && o.CheckInDate.HasValue
                       && o.CheckInDate.Value >= startOfWeek
                       && o.CheckInDate.Value < endOfWeek)
                .GroupBy(o => o.CheckInDate.Value.Date)
                .Select(g => new WeeklyRevenue
                {
                    WeekStartDate = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
                .OrderBy(wr => wr.WeekStartDate)
                .ToListAsync();
            var allDays = Enumerable.Range(0, 7)
                .Select(i => startOfWeek.AddDays(i))
                .ToList();

            var result = allDays.GroupJoin(weeklyRevenue,
                day => day,
                wr => wr.WeekStartDate,
                (day, wrs) => wrs.FirstOrDefault() ?? new WeeklyRevenue { WeekStartDate = day, Revenue = 0 })
                .OrderBy(wr => wr.WeekStartDate);

            return result;
        }

        public async Task<IEnumerable<MonthlyRevenue>> getCurrentMonthOfYearRevenueHotelBySupplierId(int supplierId)
        {
            var currentYear = DateTime.Today.Year;
            var startOfYear = new DateTime(currentYear, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var monthlyRevenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId
                       && o.Process == "Success"
                       && o.Completed == true
                       && o.CheckInDate.HasValue
                       && o.CheckInDate.Value >= startOfYear
                       && o.CheckInDate.Value < endOfYear)
                .GroupBy(o => o.CheckInDate.Value.Month)
                .Select(g => new MonthlyRevenue
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
                .OrderBy(mr => mr.Month)
                .ToListAsync();

            var allMonths = Enumerable.Range(1, 12)
                .Select(month => new MonthlyRevenue
                {
                    Month = month,
                    Revenue = monthlyRevenue.FirstOrDefault(mr => mr.Month == month)?.Revenue ?? 0
                })
                .OrderBy(mr => mr.Month);

            return allMonths;
        }

        

        public async Task<IActionResult> ToggleStatus(ToggleOrderHotelHeaderRequest request)
        {
            var orderHotelHeader = await _dbContext.OrderHotelHeaders.FindAsync(request.Id);
            if (orderHotelHeader == null)
            {
                return new NotFoundResult();
            }
            orderHotelHeader.Completed = !orderHotelHeader.Completed;
            _dbContext.Entry(orderHotelHeader).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return new NoContentResult();
        }

        public async Task<IEnumerable<QuarterlyRevenue>> getRevenueQuarterOfYearHotelBySupplierId(int supplierId, int year)
        {
            var revenue = await _dbContext.OrderHotelHeaders
        .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckInDate.Value.Year == year)
        .GroupBy(s => (s.CheckInDate.Value.Month - 1) / 3 + 1)
        .Select(g => new QuarterlyRevenue
        {
            Quarter = g.Key,
            Revenue = g.Sum(s => s.TotalPrice ?? 0)
        })
        .ToListAsync();

            // Ensure all quarters are present
            var allQuarters = Enumerable.Range(1, 4).Select(q => new QuarterlyRevenue { Quarter = q, Revenue = 0 }).ToList();
            foreach (var quarterRevenue in revenue)
            {
                var quarter = allQuarters.First(q => q.Quarter == quarterRevenue.Quarter);
                quarter.Revenue = quarterRevenue.Revenue;
            }

            return allQuarters;
        }
        public async Task<IEnumerable<RevenueHotelDateRange>> getRevenueHotelBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate)
        {
            var revenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId
                       && o.Process == "Success"
                       && o.Completed == true
                       && o.CheckInDate.HasValue
                       && o.CheckInDate.Value >= startDate
                       && o.CheckInDate.Value <= endDate)
                .GroupBy(o => o.CheckInDate.Value.Date)
                .Select(g => new RevenueHotelDateRange
                {
                    DateRange = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
                .OrderBy(r => r.DateRange)
                .ToListAsync();

            return revenue;
        }

        public async Task<IEnumerable<RevenueHotelMonthToYear>> getRevenueHotelMonthToYearBySupplierId(int supplierId, int year)
        {
            var revenue = await _dbContext.OrderHotelHeaders
        .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckInDate.Value.Year == year)
        .GroupBy(s => s.CheckInDate.Value.Month)
        .Select(g => new RevenueHotelMonthToYear
        {
            Month = g.Key,
            Revenue = g.Sum(s => s.TotalPrice ?? 0)
        })
        .ToListAsync();

            // Ensure all months are present
            var allMonths = Enumerable.Range(1, 12).Select(m => new RevenueHotelMonthToYear { Month = m, Revenue = 0 }).ToList();
            foreach (var monthRevenue in revenue)
            {
                var month = allMonths.First(m => m.Month == monthRevenue.Month);
                month.Revenue = monthRevenue.Revenue;
            }

            return allMonths;
        }
    }
}
