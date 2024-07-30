using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class ReportSupplier
    {
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public bool Status { get; set; }
        public string? Email { get; set; }
        public int ActiveTours { get; set; }
        public int ActiveHotels { get; set; }
        public int ActiveRooms { get; set; }
        public int TourBookings { get; set; }
        public int HotelBookings { get; set; }
        public decimal TourRevenue { get; set; }
        public decimal HotelRevenue { get; set; }
    }
}
