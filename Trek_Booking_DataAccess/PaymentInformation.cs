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
        [Required(ErrorMessage = "PaymentMethod is not null")]
        public string? PaymentMethod { get; set; }
        //[RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid Cart Number")]
        public float CartNumber { get; set; }
        [Required(ErrorMessage = "TotalPrice is not null")]

        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "TotalPrice is not null")]
        public decimal PaymentFee { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? PaidDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
