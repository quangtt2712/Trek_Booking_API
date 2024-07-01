using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("OrderTourHeader")]
    public class OrderTourHeader
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User? User { get; set; }


        public decimal? TotalPrice { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? TourOrderDate { get; set; }

        public string? SessionId { get; set; }

        public string? PaymentIntentId { get; set; }


        public string? FullName { get; set; }


        public string? Email { get; set; }


        public string? Phone { get; set; }

     
    }
}
