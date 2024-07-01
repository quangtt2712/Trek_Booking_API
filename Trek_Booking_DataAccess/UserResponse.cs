using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class UserResponse
    {
        public bool IsAuthSuccessful { get; set; }


        public string? ErrorMessage { get; set; }


        public string? ToKen { get; set; }

        public string? UserName { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}