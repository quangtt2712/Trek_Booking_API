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
    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is not null")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string? Email { get; set; }

        [StringLength(11, ErrorMessage = "The Phone must be equal 11 number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(300, ErrorMessage = "The Address must be less than or equal 300")]
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsVerify { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        [JsonIgnore]
        public ICollection<PaymentInformation>? paymentInformations { get; set; }
        [JsonIgnore]
        public ICollection<CartTour>? cartTours { get; set; }
        [JsonIgnore]
        public ICollection<TourOrder>? tourOrders { get; set; }
        [JsonIgnore]
        public ICollection<Comment>? comments { get; set; }
        [JsonIgnore]
        public ICollection<Rate>? rates { get; set; }
        [JsonIgnore]
        public ICollection<BookingCart>? bookingCarts { get; set; }
        [JsonIgnore]
        public ICollection<Booking>? bookings { get; set; }
        [JsonIgnore]
        public ICollection<VoucherUsageHistory>? voucherUsageHistories { get; set; }
    }
}
