using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface ISupplierStaffRepository
    {
        public Task<SupplierStaff> createSupplierStaff(SupplierStaff supplierStaff);
        public Task<SupplierStaff> updateSupplierStaff(SupplierStaff supplierStaff);
        public Task<int> deleteSupplierStaff(int staffId);
        public Task<SupplierStaff> getSupplierStaffbyId(int staffId);
        public Task<IEnumerable<SupplierStaff>> getSupplierStaffBySupplierId(int supplierId);
        public Task<IEnumerable<SupplierStaff>> getSupplierStaffs();
        public Task<SupplierStaff> checkBannedSupplierStaff(SupplierStaff supplierStaff);
        public Task<SupplierStaff> getUserByEmail(string email);
        public Task<bool> checkExitsEmail(string email);
        Task<IActionResult> ToggleStatus(ToggleSupplierStaffRequest request);
    }
}