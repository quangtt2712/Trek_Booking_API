using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDBContext _context;

        public HotelRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> checkExitsEmail(string email)
        {
            var check = await _context.hotels.AnyAsync(t => t.HotelEmail == email);
            return check;
        }

    
        public async Task<Hotel> createHotel(Hotel hotel)
        {
            hotel.IsVerify = true;
            hotel.Lock = true;
            _context.hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<int> deleteHotel(int hotelId)
        {
            var deleteHotel = await _context.hotels.FirstOrDefaultAsync(t => t.HotelId == hotelId);
            if (deleteHotel != null)
            {
                deleteHotel.Lock = true;
                _context.hotels.Update(deleteHotel);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Hotel> getHotelbyId(int hotelId)
        {
            var getHotel = await _context.hotels.FirstOrDefaultAsync(t => t.HotelId == hotelId);
            return getHotel;
        }

        public async Task<IEnumerable<Hotel>> getHotels()
        {
            var hotels = await _context.hotels.Include(s => s.Supplier).Where(h=> h.Lock == false).ToListAsync();
            return hotels;
        }
        public async Task<IEnumerable<Hotel>> getHotelsBySupplierId(int supplierId)
        {
            var hotelsBySupp = await _context.hotels.Where(s => s.SupplierId == supplierId).ToListAsync();
            return hotelsBySupp;
        }

        public async Task<int> recoverHotelDeleted(int hotelId)
        {
            var deleteHotel = await _context.hotels.FirstOrDefaultAsync(t => t.HotelId == hotelId);
            if (deleteHotel != null)
            {
                deleteHotel.Lock = false;
                _context.hotels.Update(deleteHotel);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Hotel>> searchHotelByName(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Search key cannot be null or empty", nameof(key));
            }

            var hotels = await _context.hotels.ToListAsync();

            var result = hotels.Where(h => h.HotelName.Contains(key, StringComparison.OrdinalIgnoreCase));
            return result;
        }

        public async Task<Hotel> updateHotel(Hotel hotel)
        {
            var findHotel = await _context.hotels.FirstOrDefaultAsync(t => t.HotelId == hotel.HotelId);
            if (findHotel != null)
            {
                findHotel.HotelName = hotel.HotelName;
                findHotel.HotelPhone = hotel.HotelPhone;
                findHotel.HotelEmail = hotel.HotelEmail;
                findHotel.HotelAvatar = hotel.HotelAvatar;
                findHotel.HotelFulDescription = hotel.HotelFulDescription;
                findHotel.HotelDistrict = hotel.HotelDistrict;
                findHotel.HotelCity = hotel.HotelCity;
                findHotel.HotelInformation = hotel.HotelInformation;
                findHotel.SupplierId = hotel.SupplierId;
                _context.hotels.Update(findHotel);
                await _context.SaveChangesAsync();
                return findHotel;
            }
            return null;
        }

        public async Task<Hotel> updateHotelAvatar(Hotel hotel)
        {
            var findHotel = await _context.hotels.FirstOrDefaultAsync(t => t.HotelId == hotel.HotelId);
            if (findHotel != null)
            {
                // Kiểm tra xem SupplierId có hợp lệ hay không
                var supplierExists = await _context.suppliers.AnyAsync(s => s.SupplierId == findHotel.SupplierId);
                if (!supplierExists)
                {
                    throw new InvalidOperationException("Invalid SupplierId.");
                }

                // Cập nhật trường HotelAvatar
                findHotel.HotelAvatar = hotel.HotelAvatar;

                // Giữ nguyên các giá trị khác
                findHotel.HotelName = findHotel.HotelName;
                findHotel.HotelPhone = findHotel.HotelPhone;
                findHotel.HotelEmail = findHotel.HotelEmail;
                findHotel.HotelFulDescription = findHotel.HotelFulDescription;
                findHotel.HotelDistrict = findHotel.HotelDistrict;
                findHotel.HotelCity = findHotel.HotelCity;
                findHotel.HotelInformation = findHotel.HotelInformation;
                findHotel.SupplierId = findHotel.SupplierId;

                // Chỉ cập nhật trường HotelAvatar
                _context.hotels.Update(findHotel);
                await _context.SaveChangesAsync();
                return findHotel;
            }
            return null;
        }
        public async Task<IEnumerable<Hotel>> SearchHotelByCity(string city)
        {
            var hotels = await _context.hotels
                .Where(h => EF.Functions.Like(h.HotelCity, $"%{city}%"))
                .ToListAsync();

            return hotels;
        }




        ////search by schedule
        public async Task<IEnumerable<Hotel>> SearchHotelSchedule(DateTime checkInDate, DateTime checkOutDate, string city)
        {
            // Lấy tất cả các phòng và các booking của chúng
            var rooms = await _context.rooms
                .Include(r => r.bookings)
                .Where(r => r.RoomStatus == true)
                .ToListAsync();

            // Tính toán số lượng phòng trống cho từng phòng
            var roomAvailability = rooms.Select(r => new
            {
                r.HotelId,
                r.RoomId,
                AvailableRooms = r.RoomAvailable - r.bookings
                    .Where(b => b.IsConfirmed == true &&
                                ((checkInDate >= b.CheckInDate && checkInDate < b.CheckOutDate) ||
                                 (checkOutDate > b.CheckInDate && checkOutDate <= b.CheckOutDate) ||
                                 (checkInDate <= b.CheckInDate && checkOutDate >= b.CheckOutDate)))
                    .Sum(b => b.RoomQuantity)
            }).ToList();

            // Tính toán số lượng phòng trống cho từng khách sạn
            var hotelRoomAvailability = roomAvailability
                .GroupBy(rb => rb.HotelId)
                .Select(g => new
                {
                    HotelId = g.Key,
                    AvailableRooms = g.Sum(rb => rb.AvailableRooms)
                })
                .Where(h => h.AvailableRooms > 0)
                .ToList();

            // Lấy danh sách các khách sạn có phòng trống và lọc theo thành phố
            var hotelIds = hotelRoomAvailability.Select(h => h.HotelId).ToList();
            var hotels = await _context.hotels
                .Where(h => hotelIds.Contains(h.HotelId) && EF.Functions.Like(h.HotelCity, $"%{city}%"))
                .ToListAsync();

            return hotels;
        }

        public async Task<bool> checkExitsName(string name, int supplierId)
        {
            var check = await _context.hotels.Where(x => x.SupplierId == supplierId && x.HotelName == name).AnyAsync();
            return check;
        }

        public async Task<IEnumerable<Hotel>> getHotelsByAdmin()
        {
            var hotels = await _context.hotels.Include(s => s.Supplier).ToListAsync();
            return hotels;
        }

        public async Task<IActionResult> ToggleHotel(ToggleHotelRequest request)
        {
            var hotel = await _context.hotels.FindAsync(request.HotelId);
            if (hotel == null)
            {
                return new NotFoundResult();
            }

            hotel.IsVerify = !hotel.IsVerify;
            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return new NoContentResult();
        }
    }
}
