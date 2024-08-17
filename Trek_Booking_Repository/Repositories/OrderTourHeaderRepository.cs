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
    public class OrderTourHeaderRepository : IOrderTourHeaderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderTourHeaderRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<OrderTourHeader> updateOrderTourHeader(OrderTourHeader orderTourHeader)
        {
            var findOrderTourHeader = await _dbContext.OrderTourHeaders.FirstOrDefaultAsync(b => b.Id == orderTourHeader.Id);
            if (findOrderTourHeader != null)
            {
                findOrderTourHeader.Process = orderTourHeader.Process;
                _dbContext.OrderTourHeaders.Update(findOrderTourHeader);
                await _dbContext.SaveChangesAsync();
                return findOrderTourHeader;
            }
            return null;
        }
        public async Task<IActionResult> ToggleStatus(ToggleOrderTourHeaderRequest request)
        {
            var orderTourHeader = await _dbContext.OrderTourHeaders.FindAsync(request.Id);
            if (orderTourHeader == null)
            {
                return new NotFoundResult();
            }
            orderTourHeader.Completed = !orderTourHeader.Completed;
            _dbContext.Entry(orderTourHeader).State = EntityState.Modified;

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
        public async Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderByUserId(int userId)
        {
            var find = await _dbContext.OrderTourHeaders.Where(u => u.UserId == userId && (u.Process == "Paid" || u.Process == "Success")).ToListAsync();
            return find;
        }
        public async Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderBySupplierId(int supplierId)
        {
            var check = await _dbContext.OrderTourHeaders.Where(u => u.SupplierId == supplierId && (u.Process == "Paid" || u.Process == "Success")).ToListAsync();
            return check;
        }


        public async Task<int> countTotalOrderTourBySupplierId(int supplierId)
        {
            var count = await _dbContext.OrderTourHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true).CountAsync();
            return count;
        }

        public async Task<decimal> getPercentChangeRevenueTourFromLastWeek(int supplierId, DateTime date)
        {
            // Get the current week's revenue
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekRevenue = await _dbContext.OrderTourHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.TourOrderDate >= startOfCurrentWeek && s.TourOrderDate < endOfCurrentWeek)
                .SumAsync(t => (t.TotalPrice ?? 0) * 0.995m);

            // Get the previous week's revenue
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekRevenue = await _dbContext.OrderTourHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.TourOrderDate >= startOfPreviousWeek && s.TourOrderDate < endOfPreviousWeek)
                .SumAsync(t => (t.TotalPrice ?? 0) * 0.995m);

            // Calculate the percentage change
            if (previousWeekRevenue == 0)
            {
                if (currentWeekRevenue > 0)
                    return 999999999; // Trả về 100% nếu tuần trước không có đơn hàng và tuần này có đơn hàng
                else
                    return 0; // Trả về 0% nếu cả hai tuần đều không có đơn hàng
            }

            var percentageChange = ((currentWeekRevenue - previousWeekRevenue) / previousWeekRevenue) * 100;
            return percentageChange;
        }
        public async Task<double> getPercentChangeTourFromLastWeek(int supplierId, DateTime date)
        {
            // Get the current week's count
            var startOfCurrentWeek = date.AddDays(-(int)date.DayOfWeek);
            var endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
            var currentWeekCount = await _dbContext.OrderTourHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.TourOrderDate >= startOfCurrentWeek && s.TourOrderDate < endOfCurrentWeek)
                .CountAsync();

            // Get the previous week's count
            var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
            var endOfPreviousWeek = startOfCurrentWeek;
            var previousWeekCount = await _dbContext.OrderTourHeaders
                .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.TourOrderDate >= startOfPreviousWeek && s.TourOrderDate < endOfPreviousWeek)
                .CountAsync();

            // Calculate the percentage change
            if (previousWeekCount == 0)
            {
                if (currentWeekCount > 0)
                    return double.MaxValue; // Trả về 100% nếu tuần trước không có đơn hàng và tuần này có đơn hàng
                else
                    return 0; // Trả về 0% nếu cả hai tuần đều không có đơn hàng
            }

            var percentageChange = ((currentWeekCount - previousWeekCount) / (double)previousWeekCount) * 100;
            return percentageChange;
        }

        public async Task<IEnumerable<AnnualRevenue>> getRevenueTourBySupplierId(int supplierId)
        {
            var annualRevenue = await _dbContext.OrderTourHeaders
                .Where(o => o.SupplierId == supplierId && o.Process == "Success" && o.Completed == true)
                .GroupBy(o => o.TourOrderDate.Value.Year)
                .Select(g => new AnnualRevenue
                {
                    Year = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice ?? 0)
                })
                .OrderBy(x => x.Year)
                .ToListAsync();

            return annualRevenue;
        }

        public async Task<decimal> getTotalRevenueTourBySupplierId(int supplierId)
        {
            var totalRevenue = await _dbContext.OrderTourHeaders
                .Where(u => u.SupplierId == supplierId && u.Process == "Success" && u.Completed == true)
                .SumAsync(t => (t.TotalPrice ?? 0) * 0.995m);
            return totalRevenue;
        }

        public async Task<IEnumerable<WeeklyRevenue>> getCurrentWeekRevenueTourBySupplierId(int supplierId)
        {
            var today = DateTime.Today;
            var startofweek = today.AddDays(-(int)today.DayOfWeek);
            var endofweek = startofweek.AddDays(7);

            var supplier = await _dbContext.suppliers
                .Where(s => s.SupplierId == supplierId)
                .Select(s => new { s.Commission })
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                throw new Exception("Supplier not found");
            }

            var weeklyrevenue = await _dbContext.OrderTourHeaders
                .Where(order => order.SupplierId == supplierId
                       && order.Process == "success"
                       && order.Completed == true
                       && order.TourOrderDate.HasValue
                       && order.TourOrderDate.Value >= startofweek
                       && order.TourOrderDate.Value < endofweek)
                .Select(order => new
                {
                    CheckInDate = order.TourOrderDate.Value.Date,
                    NetRevenue = (order.TotalPrice ?? 0) * 0.995m
                })
                .ToListAsync();

            var groupedRevenue = weeklyrevenue
                .GroupBy(order => order.CheckInDate)
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

            // Định dạng kết quả
            result.ForEach(r => r.Revenue = decimal.Parse(r.Revenue.ToString("F2")));

            return result;
        }
        public async Task<IEnumerable<MonthlyRevenue>> getCurrentMonthOfYearRevenueTourBySupplierId(int supplierId)
        {
            var currentYear = DateTime.Today.Year;
            var startOfYear = new DateTime(currentYear, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var monthlyRevenue = await _dbContext.OrderTourHeaders
                .Where(o => o.SupplierId == supplierId
                       && o.Process == "Success"
                       && o.Completed == true
                       && o.TourOrderDate.HasValue
                       && o.TourOrderDate.Value >= startOfYear
                       && o.TourOrderDate.Value < endOfYear)
                .GroupBy(o => o.TourOrderDate.Value.Month)
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


        public async Task<IEnumerable<QuarterlyRevenue>> getRevenueQuarterOfYearTourBySupplierId(int supplierId, int year)
        {
            var revenue = await _dbContext.OrderTourHeaders
        .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true && s.TourOrderDate.Value.Year == year)
        .GroupBy(s => (s.TourOrderDate.Value.Month - 1) / 3 + 1)
        .Select(g => new QuarterlyRevenue
        {
            Quarter = g.Key,
            Revenue = g.Sum(s => (s.TotalPrice ?? 0) * 0.995m)
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
        public async Task<IEnumerable<RevenueTourDateRange>> getRevenueTourBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate)
        {
            var revenue = await _dbContext.OrderTourHeaders
                .Where(o => o.SupplierId == supplierId
                       && o.Process == "Success"
                       && o.Completed == true
                       && o.TourOrderDate.HasValue
                       && o.TourOrderDate.Value >= startDate
                       && o.TourOrderDate.Value <= endDate)
                .GroupBy(o => o.TourOrderDate.Value.Date)
                .Select(g => new RevenueTourDateRange
                {
                    DateRange = g.Key,
                    Revenue = g.Sum(o => (o.TotalPrice ?? 0) * 0.995m)
                })
                .OrderBy(r => r.DateRange)
                .ToListAsync();
            revenue.ForEach(r => r.Revenue = decimal.Parse(r.Revenue.ToString("F2")));

            return revenue;
        }

        public async Task<IEnumerable<RevenueTourMonthToYear>> getRevenueTourMonthToYearBySupplierId(int supplierId, int year)
        {
            var revenue = await _dbContext.OrderTourHeaders
        .Where(s => s.SupplierId == supplierId && s.Process == "Success" && s.Completed == true
        && s.TourOrderDate.Value.Year == year)
        .GroupBy(s => s.TourOrderDate.Value.Month)
        .Select(g => new RevenueTourMonthToYear
        {
            Month = g.Key,
            Revenue = g.Sum(s => (s.TotalPrice ?? 0) * 0.995m)
        })
        .ToListAsync();

            // Ensure all months are present
            var allMonths = Enumerable.Range(1, 12).Select(m => new RevenueTourMonthToYear { Month = m, Revenue = 0 }).ToList();
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
