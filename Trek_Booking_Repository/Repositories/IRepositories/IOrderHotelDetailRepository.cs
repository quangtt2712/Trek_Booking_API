using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IOrderHotelDetailRepository
    {
        public Task<OrderHotelDetail> getOrderHotelDetailByOrderHotelHeaderId(int orderHotelHeaderId);
    }
}
