using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class SupplierStaffResponse
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ToKen { get; set; }
        public string? TokenSupplier { get; set; }
        public string? StaffName { get; set; }
        public string? RoleName { get; set; }
    }
}