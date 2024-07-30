using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class ReportRevenueAdmin
    {
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public bool Status { get; set; }
        public double Commission { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueAfterFee { get; set; }
        public decimal CommissionFeeReceived { get; set; }

    }
}
