using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_Repository.Repositories.IRepositories
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool Verify(string passwordHash, string inputPassword);
    }
}
