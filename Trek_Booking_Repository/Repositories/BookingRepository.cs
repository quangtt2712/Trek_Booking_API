using Microsoft.EntityFrameworkCore;
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
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDBContext _context;

        public BookingRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> checkBookingExists(int userId, int roomId)
        {
            return await _context.bookings
                .AnyAsync(t => t.UserId == userId && t.RoomId == roomId);
        }

        public async Task<Booking> createBooking(Booking booking)
        {
            var findRoom = await _context.rooms.FindAsync(booking.RoomId);
            if (findRoom == null)
            {
                throw new Exception("Room not found");
            }

            booking.HotelId = findRoom.HotelId;
            booking.Status = true;
            _context.bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<int> deleteBooking(int bookingId)
        {
            var check = await _context.bookings.FirstOrDefaultAsync(t => t.BookingId == bookingId);
            if (check != null)
            {
                check.Status = false;
                _context.bookings.Update(check);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Booking>> getBookingByHotelId(int hotelId)
        {
            var booking = await _context.bookings.Where(t => t.HotelId == hotelId).ToListAsync();
            return booking;
        }

        public async Task<Booking> getBookingById(int bookingId)
        {
            var findBCart = await _context.bookings.FirstOrDefaultAsync(t => t.BookingId == bookingId);
            return findBCart;
        }

        public async Task<IEnumerable<Booking>> getBookingByRoomId(int roomId)
        {
            var check = await _context.bookings.Where(t => t.RoomId == roomId).ToListAsync();
            return check;
        }

        public async Task<IEnumerable<Booking>> getBookingByUserId(int userId)
        {
            var check = await _context.bookings.Where(t => t.UserId == userId).ToListAsync();
            return check;
        }

        public async Task<IEnumerable<Booking>> getBookings()
        {
            var listCart = await _context.bookings.Include(t => t.User).Include(h => h.Hotel)
                .Include(r => r.Room).ToListAsync();
            return listCart;
        }

        public async Task<int> recoverBookingDeleted(int bookingId)
        {
            var check = await _context.bookings.FirstOrDefaultAsync(t => t.BookingId == bookingId);
            if (check != null)
            {
                check.Status = true;
                _context.bookings.Update(check);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Booking>> getBookingBySupplierId(int supplierId)
        {
            var bookings = await _context.bookings.Include(b => b.User).Include(b => b.Room).Include(b => b.Hotel)
               .ThenInclude(b => b.Supplier).Where(b => b.Hotel.Supplier.SupplierId == supplierId).ToListAsync();
            if (bookings.Any())
            {
                return bookings;
            }
            return null;
        }
    }
}
