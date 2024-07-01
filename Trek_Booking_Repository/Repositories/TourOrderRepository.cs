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
    public class TourOrderRepository : ITourOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public TourOrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> checkTourOders(int userId, int tourId)
        {
            return await _context.tourOrders.AnyAsync(t => t.UserId == userId && t.TourId == tourId);

        }

        public async Task<TourOrder> createTourOrder(TourOrder tourOder)
        {
            var findSupp = await _context.tours.FindAsync(tourOder.TourId);
            if (findSupp == null)
            {
                throw new Exception("Tour not found");
            }

            tourOder.SupplierId = findSupp.SupplierId;
            tourOder.Status = true;
            _context.tourOrders.Add(tourOder);
            await _context.SaveChangesAsync();
            return tourOder;
        }

        public async Task<TourOrder> deleteTourOder(TourOrder tourOrder)
        {
            var findTourOrder = await _context.tourOrders.FirstOrDefaultAsync(t => t.TourOrderId == tourOrder.TourOrderId);
            if (findTourOrder != null)
            {
                findTourOrder.Status = false;
                _context.tourOrders.Update(findTourOrder);
                await _context.SaveChangesAsync();
                return findTourOrder;
            }
            throw new Exception("Cant not Delete");
        }

        public async Task<TourOrder> getTourOrderById(int tourOrderId)
        {
            var tourOrder = await _context.tourOrders.FirstOrDefaultAsync(t => t.TourOrderId == tourOrderId);
            if (tourOrder != null)
            {
                return tourOrder;

            }
            throw new Exception("Not found");
        }

        public async Task<IEnumerable<TourOrder>> getTourOrderBySupplierId(int supplierId)
        {
            var tourOrder = await _context.tourOrders.Include(t => t.User).Include(t => t.Supplier).Include(t => t.Tour).Where(t => t.SupplierId == supplierId).ToListAsync();
            if (tourOrder != null)
            {
                return tourOrder;
            }
            throw new Exception("Not found");
        }

        public async Task<IEnumerable<TourOrder>> getTourOrderByTourId(int tourId)
        {
            var tourOrder = await _context.tourOrders.Where(t => t.TourId == tourId).ToListAsync();
            if (tourOrder != null)
            {
                return tourOrder;
            }
            throw new Exception("Not found");
        }

        public async Task<IEnumerable<TourOrder>> getTourOrderByUserId(int userId)
        {
            var tourOrder = await _context.tourOrders.Where(t => t.UserId == userId).ToListAsync();
            if (tourOrder != null)
            {
                return tourOrder;
            }
            throw new Exception("Not found");
        }

        public async Task<IEnumerable<TourOrder>> getTourOrders()
        {
            var tourOrders = await _context.tourOrders.Include(t => t.User)
                .Include(s => s.Supplier).Include(u => u.Tour).ToListAsync();
            if (tourOrders == null)
            {
                throw new Exception("Not found");
            }
            return tourOrders;
        }
    }
}
