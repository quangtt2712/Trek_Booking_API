using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("OrderHotelHeader")]
    public class OrderHotelHeader
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? SupplierId { get; set; }
        public decimal? TotalPrice { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CheckInDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CheckOutDate { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Requirement { get; set; }
        public string? VoucherCode { get; set; }
        public int? VoucherId { get; set; }

        public string? Process { get; set; }
        public bool Completed { get; set; }
    }
}
