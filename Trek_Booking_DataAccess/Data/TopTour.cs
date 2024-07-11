using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess.Data
{
    public class TopTour
    {
        public int TourId { get; set; }
        public string? TourName { get; set; }
        public int OrderCount { get; set; }
    }
}
