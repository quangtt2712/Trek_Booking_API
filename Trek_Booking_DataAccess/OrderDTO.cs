using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class OrderDTO
    {
        public OrderHotelHeader? OrderHeader { get; set; }
        public List<OrderHotelDetail>? OrderDetails { get; set; }
    }
}
