using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IOrderTourHeaderRepository
    {
        public Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderByUserId(int userId);
        public Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderBySupplierId(int supplierId);
        public Task<IEnumerable<AnnualRevenue>> getRevenueTourBySupplierId(int supplierId);
        public Task<int> countTotalOrderTourBySupplierId(int supplierId);
        public Task<double> getPercentChangeTourFromLastWeek(int supplierId, DateTime date);
        public Task<decimal> getTotalRevenueTourBySupplierId(int supplierId);
        public Task<decimal> getPercentChangeRevenueTourFromLastWeek(int supplierId, DateTime date);
        public Task<IEnumerable<WeeklyRevenue>> getCurrentWeekRevenueTourBySupplierId(int supplierId);
        public Task<IEnumerable<MonthlyRevenue>> getCurrentMonthOfYearRevenueTourBySupplierId(int supplierId);
        public Task<IEnumerable<QuarterlyRevenue>> getRevenueQuarterOfYearTourBySupplierId(int supplierId, int year);
        public Task<IEnumerable<RevenueTourDateRange>> getRevenueTourBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<RevenueTourMonthToYear>> getRevenueTourMonthToYearBySupplierId(int supplierId, int year);
        Task<IActionResult> ToggleStatus(ToggleOrderTourHeaderRequest request);
        public Task<OrderTourHeader> updateOrderTourHeader(OrderTourHeader orderTourHeader);
    }
}
