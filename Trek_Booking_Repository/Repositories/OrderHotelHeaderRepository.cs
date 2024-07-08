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
    public class OrderHotelHeaderRepository : IOrderHotelHeaderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderHotelHeaderRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderByUserId(int userId)
        {
            var check = await _dbContext.OrderHotelHeaders.Where(u => u.UserId == userId).ToListAsync();
            return check;
        }
        public async Task<IEnumerable<OrderHotelHeader>> getOrderHotelHeaderBySupplierId(int supplierId)
        {
            var check = await _dbContext.OrderHotelHeaders.Where(u => u.SupplierId == supplierId && u.Process== "Success").ToListAsync();
            return check;
        }
    }
}
