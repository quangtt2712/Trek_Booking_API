using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IRateRepository
    {
        public Task<Rate> rateHotel(Rate rate);
        public Task<IEnumerable<Rate>> getRateByHotelId(int hotelId);

        public Task<IEnumerable<Rate>> getRateByUserId(int userId);

        public Task<IEnumerable<Rate>> getRateByOrderHotelHeaderId(int OrderHotelHeaderId);

        public Task<float> getTotalRateValueByHotelId(int hotelId);

    }
}
