using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;
using Trek_Booking_DataAccess.Data;
using Trek_Booking_Repository.Repositories.IRepositories;

namespace Trek_Booking_Repository.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordHasher _passwordHasher;


        public SupplierRepository(ApplicationDBContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<Supplier> changePasswordSupplier(Supplier supplier)
        {
            var findSupplier = await _context.suppliers.FirstOrDefaultAsync(t => t.SupplierId == supplier.SupplierId);
            if (findSupplier != null)
            {
                var newPasswordHash = _passwordHasher.HashPassword(supplier.Password);
                findSupplier.Password = newPasswordHash;

                _context.suppliers.Update(findSupplier);
                await _context.SaveChangesAsync();
                return findSupplier;
            }
            return null;
        }
        public async Task<Supplier> checkPasswordSupplier(Supplier supplier)
        {
            var check = await getUserByEmail(supplier.Email);
            if (check == null)
            {
                throw new Exception("Email is not found!");
            }
            var result = _passwordHasher.Verify(check.Password, supplier.Password);
            if (!result)
            {
                return null; //Password incorrect
            }
            return check;
        }
        public async Task<bool> checkExitsEmail(string email)
        {
            var check = await _context.suppliers.AnyAsync(n => n.Email == email);
            return check;
        }
        public async Task<Supplier> checkBannedSupplier(Supplier supplier)
        {
            var userStatus = await _context.suppliers.FirstOrDefaultAsync(u => u.Status == supplier.Status);
            return userStatus;
        }
        public async Task<Supplier> getUserByEmail(string email)
        {
            var existingUser = await _context.suppliers.FirstOrDefaultAsync(u => u.Email == email);
            return existingUser;
        }
        public async Task<Supplier> createSupplier(Supplier registerRequest)
        {
            var existingUser = await getUserByEmail(registerRequest.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists.");
            }
            var passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

            // Add the new user to the database
            var supplier = new Supplier
            {
                SupplierName = registerRequest.SupplierName,
                CreateDate = DateTime.Now,
                Email = registerRequest.Email,
                Password = passwordHash,
                Address = registerRequest.Address,
                Phone = registerRequest.Phone,
                Status = true,
                IsVerify = true,
                Commission = 0.5,
                RoleId = registerRequest.RoleId,
                BankName = registerRequest.BankName,
                BankAccount = registerRequest.BankAccount,
                BankNumber = registerRequest.BankNumber,
            };
            _context.suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<int> deleteSupplier(int supplierId)
        {
            var deleteSupplier = await _context.suppliers.FirstOrDefaultAsync(t => t.SupplierId == supplierId);
            if (deleteSupplier != null)
            {
                _context.suppliers.Remove(deleteSupplier);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Supplier> getSupplierbyId(int supplierId)
        {
            var getSupplier = await _context.suppliers.FirstOrDefaultAsync(t => t.SupplierId == supplierId);
            return getSupplier;
        }

        public async Task<IEnumerable<Supplier>> getSuppliers()
        {
            var suppliers = await _context.suppliers.ToListAsync();
            return suppliers;
        }

        public async Task<Supplier> updateSupplier(Supplier supplier)
        {
            var findSupplier = await _context.suppliers.FirstOrDefaultAsync(t => t.SupplierId == supplier.SupplierId);

            if (findSupplier != null)
            {
                findSupplier.SupplierName = supplier.SupplierName;
                findSupplier.Email = supplier.Email;
                findSupplier.Phone = supplier.Phone;
                findSupplier.Address = supplier.Address;
                findSupplier.Avatar = supplier.Avatar;
                findSupplier.BankName = supplier.BankName;
                findSupplier.BankAccount = supplier.BankAccount;
                findSupplier.BankNumber = supplier.BankNumber;
                _context.suppliers.Update(findSupplier);
                await _context.SaveChangesAsync();
                return findSupplier;


            }
            return null;
        }
        public async Task<IActionResult> ToggleStatus(ToggleSupplierRequest request)
        {
            var supplier = await _context.suppliers.FindAsync(request.SupplierId);
            if (supplier == null)
            {
                return new NotFoundResult();
            }

            supplier.Status = !supplier.Status;
            _context.Entry(supplier).State = EntityState.Modified;

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
