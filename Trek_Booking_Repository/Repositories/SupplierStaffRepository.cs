using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_DataAccess;
using Trek_Booking_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Trek_Booking_Repository.Repositories
{
    public class SupplierStaffRepository : ISupplierStaffRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public SupplierStaffRepository(ApplicationDBContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<SupplierStaff> checkBannedSupplierStaff(SupplierStaff supplierStaff)
        {
            var userStatus = await _context.supplierStaff.FirstOrDefaultAsync(u => u.Status == supplierStaff.Status);
            return userStatus;
        }
        public async Task<SupplierStaff> getUserByEmail(string email)
        {
            var existingUser = await _context.supplierStaff.FirstOrDefaultAsync(u => u.StaffEmail == email);
            return existingUser;
        }
        public async Task<bool> checkExitsEmail(string email)
        {
            var check = await _context.supplierStaff.AnyAsync(n => n.StaffEmail == email);
            return check;
        }

        public async Task<SupplierStaff> createSupplierStaff(SupplierStaff supplierStaff)
        {
            var hashPassword = _passwordHasher.HashPassword(supplierStaff.StaffPassword);
            supplierStaff.Status = true;
            var supStaff = new SupplierStaff
            {
                StaffName = supplierStaff.StaffName,
                StaffPhoneNumber = supplierStaff.StaffPhoneNumber,
                StaffEmail = supplierStaff.StaffEmail,
                StaffPassword = hashPassword,
                StaffAddress = supplierStaff.StaffAddress,
                Status = true,
                IsVerify = true,
                SupplierId = supplierStaff.SupplierId,
                RoleId = supplierStaff.RoleId,
            };
            _context.supplierStaff.Add(supStaff);
            await _context.SaveChangesAsync();
            return supStaff;

        }

        public async Task<int> deleteSupplierStaff(int staffId)
        {
            var deleteSupplierStaff = await _context.supplierStaff.FirstOrDefaultAsync(t => t.StaffId == staffId);
            if (deleteSupplierStaff != null)
            {
                _context.supplierStaff.Remove(deleteSupplierStaff);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<SupplierStaff> getSupplierStaffbyId(int staffId)
        {
            var getSupplierStaff = await _context.supplierStaff.Include(s => s.Supplier).FirstOrDefaultAsync(t => t.StaffId == staffId);
            return getSupplierStaff;
        }
        public async Task<IEnumerable<SupplierStaff>> getSupplierStaffBySupplierId(int supplierId)
        {
            var getSupplierStaffBySupplierId = await _context.supplierStaff.Include(s => s.Supplier).Where(t => t.SupplierId == supplierId).ToListAsync();
            return getSupplierStaffBySupplierId;
        }
        public async Task<IEnumerable<SupplierStaff>> getSupplierStaffs()
        {
            var supplierStaff = await _context.supplierStaff.Include(s => s.Supplier).ToListAsync();
            return supplierStaff;
        }

        public async Task<SupplierStaff> updateSupplierStaff(SupplierStaff supplierStaff)
        {
            var findSupplierStaff = await _context.supplierStaff.FirstOrDefaultAsync(t => t.StaffId == supplierStaff.StaffId);
            if (findSupplierStaff != null)
            {
                findSupplierStaff.StaffName = supplierStaff.StaffName;
                findSupplierStaff.StaffPhoneNumber = supplierStaff.StaffPhoneNumber;
                findSupplierStaff.StaffEmail = supplierStaff.StaffEmail;
                findSupplierStaff.StaffPassword = supplierStaff.StaffPassword;
                findSupplierStaff.StaffAddress = supplierStaff.StaffAddress;


                _context.supplierStaff.Update(findSupplierStaff);
                await _context.SaveChangesAsync();
                return findSupplierStaff;
            }
            return null;
        }
        public async Task<IActionResult> ToggleStatus(ToggleSupplierStaffRequest request)
        {
            var supplierStaff = await _context.supplierStaff.FindAsync(request.StaffId);
            if (supplierStaff == null)
            {
                return new NotFoundResult();
            }

            supplierStaff.Status = !supplierStaff.Status;
            _context.Entry(supplierStaff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            return new NoContentResult();
        }
    }
}