using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("PaymentInformation")]
    public class PaymentInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentInforId { get; set; }
     
        public string? PaymentMethod { get; set; }
  
        public string? CartNumber { get; set; }
        public decimal? TotalPrice { get; set; }

        public decimal? PaymentFee { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? PaidDate { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
