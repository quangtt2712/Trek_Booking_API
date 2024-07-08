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
    public class OrderTourHeaderRepository : IOrderTourHeaderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderTourHeaderRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderByUserId(int userId)
        {
            var find = await _dbContext.OrderTourHeaders.Where(u => u.UserId == userId).ToListAsync();
            return find;
        }
        public async Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderBySupplierId(int supplierId)
        {
            var check = await _dbContext.OrderTourHeaders.Where(u => u.SupplierId == supplierId).ToListAsync();
            return check;
        }
    }
}
