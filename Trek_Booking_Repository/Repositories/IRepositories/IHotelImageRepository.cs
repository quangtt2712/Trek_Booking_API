using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IHotelImageRepository
    {
        public Task<HotelImage> createHotelImage(HotelImage hotelImage);
        public Task<HotelImage> updateHotelImage(HotelImage hotelImage);
        public Task<int> deleteHotelImage(int hotelImageId);
        public Task<HotelImage> getHotelImagebyId(int hotelImageId);
        public Task<IEnumerable<HotelImage>> getHotelImages();
        public Task<IEnumerable<HotelImage>> getHotelImageByHotelId(int hotelId);
    }
}
