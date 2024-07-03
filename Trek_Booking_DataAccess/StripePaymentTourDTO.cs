using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    public class StripePaymentTourDTO
    {
        public StripePaymentTourDTO()
        {
            SuccessUrl = "booking_history";
            CancelUrl = "booking_infor";
        }
        public OrderTourDTO? Order { get; set; }
        public string? SuccessUrl { get; set; }

        public string? CancelUrl { get; set; }
    }
}
