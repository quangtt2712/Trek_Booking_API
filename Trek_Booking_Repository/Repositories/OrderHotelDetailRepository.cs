using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class OrderHotelDetailRepository : IOrderHotelDetailRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderHotelDetailRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<OrderHotelDetail> getOrderHotelDetailByOrderHotelHeaderId(int orderHotelHeaderId)
        {
            var check = await _dbContext.OrderHotelDetails.FirstOrDefaultAsync(t => t.OrderHotelHeaderlId == orderHotelHeaderId);
            return check;
        }
    }
}
