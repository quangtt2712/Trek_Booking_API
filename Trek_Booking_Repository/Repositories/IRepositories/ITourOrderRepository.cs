using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ITourOrderRepository
    {
        public Task<IEnumerable<TourOrder>> getTourOrderByUserId(int userId);
        public Task<IEnumerable<TourOrder>> getTourOrderByTourId(int tourId);
        public Task<IEnumerable<TourOrder>> getTourOrderBySupplierId(int supplierId);
        public Task<TourOrder> getTourOrderById(int tourOrderId);
        public Task<TourOrder> deleteTourOder(TourOrder tourOrder);
        public Task<TourOrder> createTourOrder(TourOrder tourOder);
        public Task<bool> checkTourOders(int userId, int tourId);
        public Task<IEnumerable<TourOrder>> getTourOrders();
    }
}
