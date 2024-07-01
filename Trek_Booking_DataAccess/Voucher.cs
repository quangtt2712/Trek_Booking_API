using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherId { get; set; }

        [Required(ErrorMessage = "VoucherCode is not null")]
        [StringLength(50, ErrorMessage = "VoucherCode is not null")]
        public string? VoucherCode { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? AvailableDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ExpireDate { get; set; }

        [Required(ErrorMessage = "VoucherQuantity is not null")]
        public int VoucherQuantity { get; set; }

        [Required(ErrorMessage = "DiscountPercent is not null")]
        public float DiscountPercent { get; set; }

        public bool VoucherStatus { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public Hotel? Hotel { get; set; }

        [JsonIgnore]
        public ICollection<VoucherUsageHistory>? voucherUsageHistories { get; set; }
    }
}
