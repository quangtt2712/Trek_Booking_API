using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class SupplierResponse
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ToKen { get; set; }
        public string? SupplierName { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}