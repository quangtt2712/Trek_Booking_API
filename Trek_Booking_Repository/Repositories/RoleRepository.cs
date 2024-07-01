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
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;
        public RoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Role> createRole(Role role)
        {
            _context.roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<int> deleteRole(int roleId)
        {
            var deleteRole = await _context.roles.FirstOrDefaultAsync(t => t.RoleId == roleId);
            if (deleteRole != null)
            {
                _context.roles.Remove(deleteRole);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public  async Task<Role> getRoleById(int roleId)
        {
            var getRole = await _context.roles.FirstOrDefaultAsync(t => t.RoleId == roleId);
            return getRole;
        }

        public async Task<IEnumerable<Role>> getRoles()
        {
            var roles = await _context.roles.ToListAsync();
            return roles;
        }

        public async Task<Role> updateRole(Role role)
        {
            var findRole = await _context.roles.FirstOrDefaultAsync(t => t.RoleId == role.RoleId);
            if (findRole != null)
            {
                findRole.RoleName = role.RoleName;
                findRole.RoleDescription = role.RoleDescription;
                _context.roles.Update(findRole);
                await _context.SaveChangesAsync();
                return findRole;
            }
            return null;
        }
    }
}
