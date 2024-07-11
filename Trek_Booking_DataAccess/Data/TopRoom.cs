using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess.Data
{
    public class TopRoom
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? HotelName { get; set; }
        public int OrderCount { get; set; }

    }
}
