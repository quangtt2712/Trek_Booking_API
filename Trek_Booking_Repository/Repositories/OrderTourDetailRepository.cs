using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Trek_Booking_Repository.Repositories
{
    public class OrderTourDetailRepository : IOrderTourDetailRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderTourDetailRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<TourDateRange>> getMostFrequentlyTourBySupplierIdAndDateRange(int supplierId, DateTime startDate, DateTime endDate)
        {
            var result = await (from detail in _dbContext.OrderTourDetails
                                join header in _dbContext.OrderTourHeaders
                                on detail.OrderTourHeaderlId equals header.Id
                                join tour in _dbContext.tours
                                on detail.TourId equals tour.TourId
                                where header.TourOrderDate >= startDate && header.TourOrderDate <= endDate
                                && tour.SupplierId == supplierId && header.Process == "Success"
                                && header.Completed == true
                                group detail by new { detail.TourName } into g
                                select new TourDateRange
                                {
                                    TourName = g.Key.TourName,
                                    OrderCount = g.Count()
                                })
                           .OrderByDescending(o => o.OrderCount)
                           //.Take(5)
                           .ToListAsync();
            return result;
        }

        public async Task<OrderTourDetail> GetOrderTourDetailByOrderTourHeaderId(int orderTourHeaderId)
        {
            var check = await _dbContext.OrderTourDetails.Include(t => t.Tour).FirstOrDefaultAsync(t => t.OrderTourHeaderlId == orderTourHeaderId);
            return check;
        }

        public async Task<IEnumerable<TopTour>> getTop5TourInWeek(int supplierId, DateTime startOfWeek, DateTime endOfWeek)
        {
            var result = await (from detail in _dbContext.OrderTourDetails
                                join header in _dbContext.OrderTourHeaders
                                on detail.OrderTourHeaderlId equals header.Id
                                join tour in _dbContext.tours
                                on detail.TourId equals tour.TourId
                                where header.TourOrderDate >= startOfWeek && header.TourOrderDate <= endOfWeek
                                && tour.SupplierId == supplierId && header.Process == "Success"
                                && header.Completed == true
                                group detail by new { detail.TourId, detail.TourName } into g
                                select new TopTour
                                {
                                    TourId = g.Key.TourId,
                                    TourName = g.Key.TourName,
                                    OrderCount = g.Count()
                                })
                           .OrderByDescending(o => o.OrderCount)
                           //.Take(5)
                           .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Top5Tour>> getTop5TourOrders(int supplierId)
        {
            var top5Tours = await _dbContext.OrderTourDetails
            .Where(o => o.Tour.SupplierId == supplierId)
            .GroupBy(o => o.TourName)
            .Select(g => new Top5Tour
            {
                TourName = g.Key,
                OrderCount = g.Count()
            })
            .OrderByDescending(t => t.OrderCount)
            .Take(5)
            .ToListAsync();

            return top5Tours;
        }
    }
}
