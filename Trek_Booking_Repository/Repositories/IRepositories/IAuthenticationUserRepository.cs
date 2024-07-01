using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IAuthenticationUserRepository
    {
        public Task<User> checkPasswordClient(User user);
        public Task<Supplier> checkPasswordSupplier(Supplier supplier);
        public Task<SupplierStaff> checkPasswordSupplierStaff(SupplierStaff supplierStaff);
    }
}