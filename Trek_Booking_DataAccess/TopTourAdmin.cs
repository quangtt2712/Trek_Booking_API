using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class TopTourAdmin
    {
        public int TourId { get; set; }
        public string? TourName { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int OrderCount { get; set; }
    }
}
