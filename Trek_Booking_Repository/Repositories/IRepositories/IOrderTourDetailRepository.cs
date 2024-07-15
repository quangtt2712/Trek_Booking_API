using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IOrderTourDetailRepository
    {
        public Task<OrderTourDetail> GetOrderTourDetailByOrderTourHeaderId(int orderTourHeaderId);
        public Task<IEnumerable<Top5Tour>> getTop5TourOrders(int supplierId);
        public Task<IEnumerable<TopTour>> getTop5TourInWeek(int supplierId, DateTime startDate, DateTime endDate);

        public Task<IEnumerable<TourDateRange>> getMostFrequentlyTourBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate);
    }
}
