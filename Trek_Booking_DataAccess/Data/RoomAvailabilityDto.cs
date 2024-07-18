using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess.Data
{
    public class RoomAvailabilityDto
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public int AvailableRooms { get; set; }
    }
}
