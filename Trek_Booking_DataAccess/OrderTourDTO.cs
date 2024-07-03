using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class OrderTourDTO
    {
        public OrderTourHeader? OrderHeader { get; set; }
        public List<OrderTourDetail>? OrderDetails { get; set; }
    }
}
