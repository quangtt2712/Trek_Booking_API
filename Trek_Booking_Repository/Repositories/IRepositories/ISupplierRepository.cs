using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ISupplierRepository
    {
        public Task<Supplier> createSupplier(Supplier supplier);
        public Task<Supplier> updateSupplier(Supplier supplier);
        public Task<int> deleteSupplier(int supplierId);
        public Task<Supplier> getSupplierbyId(int supplierId);
        public Task<IEnumerable<Supplier>> getSuppliers();
        public Task<Supplier> checkBannedSupplier(Supplier supplier);
        public Task<Supplier> getUserByEmail(string email);

        public Task<bool> checkExitsEmail(string email);
        Task<IActionResult> ToggleStatus(ToggleSupplierRequest request);

        public Task<Supplier> changePasswordSupplier(Supplier supplier);
        public Task<Supplier> checkPasswordSupplier(Supplier supplier);
    }
}
