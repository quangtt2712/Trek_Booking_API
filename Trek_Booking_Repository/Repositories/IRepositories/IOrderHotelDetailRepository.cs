using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IOrderHotelDetailRepository
    {
        public Task<OrderHotelDetail> getOrderHotelDetailByOrderHotelHeaderId(int orderHotelHeaderId);

        public Task<IEnumerable<Top5Room>> getTop5RoomOrders(int supplierId);
        public Task<IEnumerable<TopRoom>> getTop5RoomInWeek(int supplierId, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<RoomDateRange>> getMostFrequentlyRoomBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate);

    }
}
