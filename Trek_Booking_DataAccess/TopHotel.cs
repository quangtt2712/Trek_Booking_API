using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class TopHotel
    {
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int OrderCount { get; set; }
    }
}
