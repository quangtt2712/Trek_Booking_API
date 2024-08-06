using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IHotelRepository
    {
        public Task<Hotel> createHotel(Hotel hotel);
        public Task<Hotel> updateHotel(Hotel hotel);
        public Task<int> deleteHotel(int hotelId);
        public Task<int> recoverHotelDeleted(int hotelId);
        public Task<Hotel> getHotelbyId(int hotelId);
        public Task<IEnumerable<Hotel>> getHotels();
        public Task<IEnumerable<Hotel>> getHotelsByAdmin();
        Task<IActionResult> ToggleHotel(ToggleHotelRequest request);
        public Task<IEnumerable<Hotel>> getHotelsBySupplierId(int supplierId);
        public Task<bool> checkExitsName(string name, int supplierId);
        public Task<bool> checkExitsEmail(string email);
        public Task<IEnumerable<Hotel>> searchHotelByName(string key);

        public Task<Hotel> updateHotelAvatar(Hotel hotel);
        Task<IEnumerable<Hotel>> SearchHotelByCity(string city);
        public Task<IEnumerable<Hotel>> SearchHotelSchedule(DateTime checkInDate, DateTime checkOutDate, string city);
    }
}
