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
    [Table("Supplier")]
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }


        [StringLength(100, ErrorMessage = "The SupplierName must be greater than 0 and less than or equal 100")]
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Email is not null")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100, ErrorMessage = "The Email must be greater than 0 and less than or equal 100")]
        public string? Email { get; set; }

        [StringLength(12, ErrorMessage = "The Phone must be equal 12 number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [StringLength(300, ErrorMessage = "The Address must be less than or equal 300")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Password is not null")]
        public string? Password { get; set; }
        public string? Avatar { get; set; }

        public bool Status { get; set; }
        public bool IsVerify { get; set; }
        public DateTime CreateDate { get; set; }
        public double Commission { get; set; }
        public string? BankName { get; set; }
        public string? BankAccount { get; set; }
        public string? BankNumber { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        [JsonIgnore]
        public ICollection<SupplierStaff>? supplierStaffs { get; set; }
        [JsonIgnore]
        public ICollection<Hotel>? hotels { get; set; }
        [JsonIgnore]
        public ICollection<Tour>? tours { get; set; }
        [JsonIgnore]
        public ICollection<TourOrder>? tourOrders { get; set; }
    }
}
