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
    public class CartTourRepository : ICartTourRepository
    {
        private readonly ApplicationDBContext _context;

        public CartTourRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> checkCartTourExists(int userId, int tourId)
        {
            return await _context.cartTours
                .AnyAsync(t => t.UserId == userId && t.TourId == tourId);
        }

        public async Task<CartTour> createCartTour(CartTour cartTour)
        {
            _context.cartTours.Add(cartTour);
            await _context.SaveChangesAsync();
            return cartTour;
        }

        public async Task<int> deleteCartTour(int cartTourId)
        {
            var check = await _context.cartTours.FirstOrDefaultAsync(t => t.CartTourId == cartTourId);
            if (check != null)
            {
                _context.cartTours.Remove(check);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CartTour> getCartTourById(int cartTourId)
        {
            var findCartTour = await _context.cartTours.FirstOrDefaultAsync(t => t.CartTourId == cartTourId);
            if (findCartTour != null)
            {
                return findCartTour;
            }
            throw new Exception("Not found");
        }

        public async Task<IEnumerable<CartTour>> getCartTourByTourId(int TourId)
        {
            var findTour = await _context.cartTours.Where(t => t.TourId == TourId).ToListAsync();
            if (findTour.Any())
            {
                return findTour;
            }
            throw new Exception("Not found");
        }
        public async Task<IEnumerable<CartTour>> getCartTourByUserId(int userId)
        {
            var findUser = await _context.cartTours.Where(t => t.UserId == userId).ToListAsync();
            return findUser;
        }

        public async Task<IEnumerable<CartTour>> getCartTours()
        {
            var findAll = await _context.cartTours.Include(t => t.Tour).Include(y => y.User).ToListAsync();
            return findAll;
        }

        public async Task<CartTour> updateCartTour(CartTour cartTour)
        {
            var find = await _context.cartTours.FirstOrDefaultAsync(t => t.CartTourId == cartTour.CartTourId);
            if (find != null)
            {
                find.TourQuantity = cartTour.TourQuantity;
                find.TourPrice = cartTour.TourPrice;
                _context.cartTours.Update(find);
                await _context.SaveChangesAsync();
                return find;
            }
            throw new Exception("Cant not Create");
        }
    }
}