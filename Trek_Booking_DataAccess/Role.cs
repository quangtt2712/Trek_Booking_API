using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("Role")]
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role not null")]
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public ICollection<User>? users { get; set; }
        public ICollection<Supplier>? suppliers { get; set; }
        public ICollection<SupplierStaff>? supplierStaffs { get; set; }
    }
}
