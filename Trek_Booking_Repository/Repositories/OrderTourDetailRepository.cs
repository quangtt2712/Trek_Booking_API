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
        public async Task<OrderTourDetail> GetOrderTourDetailByOrderTourHeaderId(int orderTourHeaderId)
        {
            var check = await _dbContext.OrderTourDetails.Include(t => t.Tour).FirstOrDefaultAsync(t => t.OrderTourHeaderlId == orderTourHeaderId);
            return check;
        }
    }
}
