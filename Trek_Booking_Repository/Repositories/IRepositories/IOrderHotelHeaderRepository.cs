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
    public interface IOrderHotelHeaderRepository
    {
        public Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderByUserId(int userId);
        public Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderBySupplierId(int supplierId);
        public Task<int> countTotalOrderHotelBySupplierId(int supplierId);
        public Task<double> getPercentChangeFromLastWeek(int supplierId);
        public Task<decimal> getTotalRevenueHotelBySupplierId(int supplierId);
        public Task<decimal> getPercentChangeRevenueFromLastWeek(int supplierId);
        public Task<IEnumerable<WeeklyRevenue>> getCurrentWeekRevenueHotelBySupplierId(int supplierId);
        public Task<IEnumerable<MonthlyRevenue>> getCurrentMonthOfYearRevenueHotelBySupplierId(int supplierId);
        public Task<IEnumerable<QuarterlyRevenue>> getRevenueQuarterOfYearHotelBySupplierId(int supplierId, int year);
        public Task<IEnumerable<RevenueHotelDateRange>> getRevenueHotelBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<RevenueHotelMonthToYear>> getRevenueHotelMonthToYearBySupplierId(int supplierId, int year);
       Task<IActionResult> ToggleStatus(ToggleOrderHotelHeaderRequest request);
        public Task<OrderHotelHeader> updateOrderHotelHeader(OrderHotelHeader orderHotelHeader);

    }
}
