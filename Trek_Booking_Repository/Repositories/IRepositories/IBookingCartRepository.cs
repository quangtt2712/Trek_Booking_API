using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IBookingCartRepository
    {
        public Task<IEnumerable<BookingCart>> getBookingCartbyUserId(int userId);
        public Task<IEnumerable<BookingCart>> getBookingCartbyHotelId(int hotelId);
        public Task<IEnumerable<BookingCart>> getBookingCartbyRoomId(int roomId);
        public Task<BookingCart> getBookingCartbyId(int bookingCartId);
        public Task<int> deleteBookingCart(int bookingCartId);
        public Task<BookingCart> createBookingCart(BookingCart bookingCart);
        public Task<bool> CheckBookingCartExists(int userId, int roomId);
        public Task<BookingCart> updateBookingCart(BookingCart bookingCart);
        public Task<IEnumerable<BookingCart>> getBookingCarts();
    }
}
