using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class StripePaymentDTO
    {
        public StripePaymentDTO()
        {
            SuccessUrl = "booking_history";
            CancelUrl = "booking_infor";
        }
        public OrderDTO? Order { get; set; }
        public string? SuccessUrl { get; set; }

        public string? CancelUrl { get; set; }
    }
}
