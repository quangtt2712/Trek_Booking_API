using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ICartTourRepository
    {
        public Task<IEnumerable<CartTour>> getCartTourByUserId(int userId);
        public Task<IEnumerable<CartTour>> getCartTourByTourId(int TourId);
        public Task<CartTour> getCartTourById(int cartTourId);
        public Task<int> deleteCartTour(int cartTourId);
        public Task<CartTour> createCartTour(CartTour cartTour);
        public Task<bool> checkCartTourExists(int userId, int tourId);
        public Task<CartTour> updateCartTour(CartTour cartTour);
        public Task<IEnumerable<CartTour>> getCartTours();
    }
}
