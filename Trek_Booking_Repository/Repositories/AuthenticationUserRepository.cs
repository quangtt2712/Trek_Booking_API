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
    public class AuthenticationUserRepository : IAuthenticationUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierStaffRepository _supplierStaffRepository;

        public AuthenticationUserRepository(ApplicationDBContext context, IPasswordHasher passwordHasher,
            IUserRepository userRepository, ISupplierRepository supplierRepository, ISupplierStaffRepository supplierStaffRepository)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _supplierRepository = supplierRepository;
            _supplierStaffRepository = supplierStaffRepository;
        }
        public async Task<User> checkPasswordClient(User loginRequest)
        {
            var user = await _userRepository.getUserByEmail(loginRequest.Email);
            if (user == null)
            {
                throw new Exception("Email is not found!");
            }
            var result = _passwordHasher.Verify(user.Password, loginRequest.Password);
            if (!result)
            {
                throw new Exception("Email or password is not correct!");
            }
            return user;
        }

        public async Task<Supplier> checkPasswordSupplier(Supplier loginRequest)
        {
            var supplier = await _supplierRepository.getUserByEmail(loginRequest.Email);
            if (supplier == null)
            {
                throw new Exception("Email is not found!");
            }
            var result = _passwordHasher.Verify(supplier.Password, loginRequest.Password);
            if (!result)
            {
                throw new Exception("Email or password is not correct!");
            }
            return supplier;
        }

        public async Task<SupplierStaff> checkPasswordSupplierStaff(SupplierStaff loginRequest)
        {
            var supplierStaff = await _supplierStaffRepository.getUserByEmail(loginRequest.StaffEmail);
            if (supplierStaff == null)
            {
                throw new Exception("Email is not found!");
            }
            var result = _passwordHasher.Verify(supplierStaff.StaffPassword, loginRequest.StaffPassword);
            if (!result)
            {
                throw new Exception("Email or password is not correct!");
            }
            return supplierStaff;
        }
    }
}