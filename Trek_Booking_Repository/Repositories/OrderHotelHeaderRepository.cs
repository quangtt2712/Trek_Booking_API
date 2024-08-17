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
        public async Task<double> getPercentChangeFromLastWeek(int supplierId)
        {
            var date = DateTime.Now;
            // Get the current week's count
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekCount = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckOutDate >= startOfCurrentWeek && s.CheckOutDate < endOfCurrentWeek)
                .CountAsync();

            // Get the previous week's count
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekCount = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckOutDate >= startOfPreviousWeek && s.CheckOutDate < endOfPreviousWeek)
                .CountAsync();

            // Calculate the percentage change
            if (previousWeekCount == 0)
            {
                if (currentWeekCount > 0)
                    return double.MaxValue; 
                else
                    return 0; // Trả về 0% nếu cả hai tuần đều không có đơn hàng
            }

            var percentageChange = ((currentWeekCount - previousWeekCount) / (double)previousWeekCount) * 100;
            return percentageChange;
        }


        public async Task<decimal> getPercentChangeRevenueFromLastWeek(int supplierId)
        {
            var date = DateTime.Now;

            // Get the current week's revenue
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekRevenue = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckOutDate >= startOfCurrentWeek && s.CheckOutDate < endOfCurrentWeek)
                .SumAsync(t => (t.TotalPrice ?? 0) * 0.995m);

            // Get the previous week's revenue
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekRevenue = await _dbContext.OrderHotelHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.CheckOutDate >= startOfPreviousWeek && s.CheckOutDate < endOfPreviousWeek)
                .SumAsync(t => (t.TotalPrice ?? 0) * 0.995m);

            // Handle edge cases for percentage change calculation
            if (previousWeekRevenue == 0)
            {
                if (currentWeekRevenue > 0)
                    return 999999999; // Đại diện cho sự tăng trưởng vô hạn
                else
                    return 0; // Trả về 0% nếu cả hai tuần đều không có doanh thu
            }

            // Calculate the percentage change
            var percentageChange = ((currentWeekRevenue - previousWeekRevenue) / previousWeekRevenue) * 100;
            return percentageChange;
        }


        public async Task<IEnumerable<AnnualRevenue>> getRevenueYearBySupplierId(int supplierId)
        {
            var annualRevenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId && o.Process == "Success" && o.Completed == true)
                .GroupBy(o => o.CheckOutDate.Value.Year)
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
                .SumAsync(t => (t.TotalPrice ?? 0) * 0.995m);
            return totalRevenue;
        }
        public async Task<IEnumerable<WeeklyRevenue>> getCurrentWeekRevenueHotelBySupplierId(int supplierid)
        {
            var today = DateTime.Today;
            var startofweek = today.AddDays(-(int)today.DayOfWeek);
            var endofweek = startofweek.AddDays(7);

            var supplier = await _dbContext.suppliers
                .Where(s => s.SupplierId == supplierid)
                .Select(s => new { s.Commission })
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                throw new Exception("Supplier not found");
            }

            var weeklyrevenue = await _dbContext.OrderHotelHeaders
                .Where(order => order.SupplierId == supplierid
                       && order.Process == "success"
                       && order.Completed == true
                       && order.CheckOutDate.HasValue
                       && order.CheckOutDate.Value >= startofweek
                       && order.CheckOutDate.Value < endofweek)
                .Select(order => new
                {
                    CheckOutDate = order.CheckOutDate.Value.Date,
                    NetRevenue = (order.TotalPrice ?? 0) * 0.995m
                })
                .ToListAsync();

            var groupedRevenue = weeklyrevenue
                .GroupBy(order => order.CheckOutDate)
                .Select(g => new WeeklyRevenue
                {
                    WeekStartDate = g.Key,
                    Revenue = g.Sum(order => order.NetRevenue)
                })
                .OrderBy(wr => wr.WeekStartDate)
                .ToList();

            var alldays = Enumerable.Range(0, 7)
                .Select(i => startofweek.AddDays(i))
                .ToList();

            var result = alldays.GroupJoin(groupedRevenue,
                day => day,
                wr => wr.WeekStartDate,
                (day, wrs) => wrs.FirstOrDefault() ?? new WeeklyRevenue { WeekStartDate = day, Revenue = 0 })
                .OrderBy(wr => wr.WeekStartDate)
                .ToList();

            result.ForEach(r => r.Revenue = decimal.Parse(r.Revenue.ToString("F2")));

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
                       && o.CheckOutDate.HasValue
                       && o.CheckOutDate.Value >= startOfYear
                       && o.CheckOutDate.Value < endOfYear)
                .GroupBy(o => o.CheckOutDate.Value.Month)
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
        .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true
        && s.CheckOutDate.Value.Year == year)
        .GroupBy(s => (s.CheckOutDate.Value.Month - 1) / 3 + 1)
        .Select(g => new QuarterlyRevenue
        {
            Quarter = g.Key,
            Revenue = g.Sum(t => (t.TotalPrice ?? 0) * 0.995m)
        })
        .ToListAsync();

            // Ensure all quarters are present
            var allQuarters = Enumerable.Range(1, 4).Select(q => new QuarterlyRevenue { Quarter = q, Revenue = 0 }).ToList();
            foreach (var quarterRevenue in revenue)
            {
                var quarter = allQuarters.First(q => q.Quarter == quarterRevenue.Quarter);
                quarter.Revenue = quarterRevenue.Revenue;
            }
            allQuarters.ForEach(r => r.Revenue = decimal.Parse(r.Revenue.ToString("F2")));

            return allQuarters;
        }
        public async Task<IEnumerable<RevenueHotelDateRange>> getRevenueHotelBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate)
        {
            var revenue = await _dbContext.OrderHotelHeaders
                .Where(o => o.SupplierId == supplierId
                       && o.Process == "Success"
                       && o.Completed == true
                       && o.CheckOutDate.HasValue
                       && o.CheckOutDate.Value >= startDate
                       && o.CheckOutDate.Value <= endDate)
                .GroupBy(o => o.CheckOutDate.Value.Date)
                .Select(g => new RevenueHotelDateRange
                {
                    DateRange = g.Key,
                    Revenue = g.Sum(o => (o.TotalPrice ?? 0) * 0.995m)
                })
                .OrderBy(r => r.DateRange)
                .ToListAsync();
            revenue.ForEach(r => r.Revenue = decimal.Parse(r.Revenue.ToString("F2")));

            return revenue;
        }

        public async Task<IEnumerable<RevenueHotelMonthToYear>> getRevenueHotelMonthToYearBySupplierId(int supplierId, int year)
        {
            var revenue = await _dbContext.OrderHotelHeaders
        .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true
        && s.CheckOutDate.Value.Year == year)
        .GroupBy(s => s.CheckOutDate.Value.Month)
        .Select(g => new RevenueHotelMonthToYear
        {
            Month = g.Key,
            Revenue = g.Sum(s => (s.TotalPrice ?? 0) * 0.995m)
        })
        .ToListAsync();

            // Ensure all months are present
            var allMonths = Enumerable.Range(1, 12).Select(m => new RevenueHotelMonthToYear { Month = m, Revenue = 0 }).ToList();
            foreach (var monthRevenue in revenue)
            {
                var month = allMonths.First(m => m.Month == monthRevenue.Month);
                month.Revenue = monthRevenue.Revenue;
            }
            allMonths.ForEach(r => r.Revenue = decimal.Parse(r.Revenue.ToString("F2")));

            return allMonths;
        }
    }
}
