using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IOrderTourHeaderRepository
    {
        public Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderByUserId(int userId);
        public Task<IEnumerable<OrderTourHeader>> getOrderTourHeaderBySupplierId(int supplierId);
    }
}
