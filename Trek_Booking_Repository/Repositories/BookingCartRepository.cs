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
    public class BookingCartRepository : IBookingCartRepository
    {
        private readonly ApplicationDBContext _context;

        public BookingCartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckBookingCartExists(int userId, int roomId)
        {
            return await _context.bookingCarts
                .AnyAsync(t => t.UserId == userId && t.RoomId == roomId);
        }

        public async Task<BookingCart> createBookingCart(BookingCart bookingCart)
        {
            var findRoom = await _context.rooms.FindAsync(bookingCart.RoomId);
            if (findRoom == null)
            {
                throw new Exception("Room not found");
            }

            bookingCart.HotelId = findRoom.HotelId;
            //bookingCart.TotalPrice = findRoom.RoomPrice * bookingCart.RoomQuantity;
            _context.bookingCarts.Add(bookingCart);
            await _context.SaveChangesAsync();
            return bookingCart;
        }

        public async Task<int> deleteBookingCart(int bookingCartId)
        {
            var cartsToDelete = await _context.bookingCarts.FirstOrDefaultAsync(t => t.BookingCartId == bookingCartId);
            if (cartsToDelete != null)
            {
                _context.bookingCarts.Remove(cartsToDelete);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<IEnumerable<BookingCart>> getBookingCartbyHotelId(int hotelId)
        {
            var booking = await _context.bookingCarts.Where(t => t.HotelId == hotelId).ToListAsync();
            return booking;
        }

        public async Task<BookingCart> getBookingCartbyId(int bookingCartId)
        {
            var findBCart = await _context.bookingCarts.FirstOrDefaultAsync(t => t.BookingCartId == bookingCartId);
            return findBCart;
        }

        public async Task<IEnumerable<BookingCart>> getBookingCartbyRoomId(int roomId)
        {
            var check = await _context.bookingCarts.Where(t => t.RoomId == roomId).ToListAsync();
            return check;
        }

        public async Task<IEnumerable<BookingCart>> getBookingCartbyUserId(int userId)
        {
            var check = await _context.bookingCarts.Where(t => t.UserId == userId).ToListAsync();
            return check;
        }

        public async Task<IEnumerable<BookingCart>> getBookingCarts()
        {
            var listCart = await _context.bookingCarts.Include(t => t.User).Include(h => h.Hotel)
                .Include(r => r.Room).ToListAsync();
            return listCart;
        }

        public async Task<BookingCart> updateBookingCart(BookingCart bookingCart)
        {
            var check = await _context.bookingCarts.FirstOrDefaultAsync(t => t.BookingCartId == bookingCart.BookingCartId);
            if (check != null)
            {
                check.CheckOutDate = bookingCart.CheckInDate;
                check.CheckOutDate = bookingCart.CheckOutDate;
                check.RoomQuantity = bookingCart.RoomQuantity;

                var findRoom = await _context.rooms.FindAsync(check.RoomId);
                if (findRoom != null)
                {
                    check.TotalPrice = bookingCart.RoomQuantity * findRoom.RoomPrice;
                }
                else
                {
                    throw new Exception("Room not found");
                }
                _context.bookingCarts.Update(check);
                await _context.SaveChangesAsync();
                return check;
            }
            return null;
        }
    }
}
