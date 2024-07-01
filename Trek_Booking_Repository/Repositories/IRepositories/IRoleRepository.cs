using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trek_Booking_DataAccess;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IRoleRepository
    {
        public Task<Role> createRole(Role role);
        public Task<Role> updateRole(Role role);
        public Task<int> deleteRole(int roleId);
        public Task<Role> getRoleById(int roleId);
        public Task<IEnumerable<Role>> getRoles();
    }
}
