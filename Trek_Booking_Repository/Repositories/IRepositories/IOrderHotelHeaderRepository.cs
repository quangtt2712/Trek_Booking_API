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
        public Task<IEnumerable<AnnualRevenue>> getRevenueYearBySupplierId(int supplierId);
        public Task<int> countTotalOrderHotelBySupplierId(int supplierId);
        public Task<double> getPercentChangeFromLastWeek(int supplierId, DateTime date);
        public Task<decimal> getTotalRevenueHotelBySupplierId(int supplierId);
        public Task<decimal> getPercentChangeRevenueFromLastWeek(int supplierId, DateTime date);
        public Task<IEnumerable<WeeklyRevenue>> getCurrentWeekRevenueHotelBySupplierId(int supplierId);
        public Task<IEnumerable<MonthlyRevenue>> getCurrentMonthOfYearRevenueHotelBySupplierId(int supplierId);
        public Task<IEnumerable<QuarterlyRevenue>> getCurrentQuarterOfYearRevenueHotelBySupplierId(int supplierId);

    }
}
