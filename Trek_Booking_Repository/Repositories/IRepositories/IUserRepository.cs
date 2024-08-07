using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IUserRepository
    {

        public Task<User> createUser(User registerRequest);
        public Task<User> updateUser(User user);
        public Task<int> deleteUser(int userId);
        public Task<int> recoverUserDeleted(int userId);
        public Task<IEnumerable<User>> getUserByRoleId(int roleId);
        public Task<bool> checkExitsEmail(string email);
        public Task<User> getUserById(int userId);
        public Task<IEnumerable<User>> getUsers();
        public Task<User> getUserByEmail(string email);
        public Task<User> checkBannedUser(User user);
        public Task<bool> checkIsAdmin(User user);
        Task<IActionResult> ToggleStatus(ToggleUserRequest request);

        public Task<User> changePasswordUser(User user);
        public Task<User> checkPasswordUser(User user);
    }
}
