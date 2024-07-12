
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("TourOrder")]
    public class TourOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TourOrderId { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [ForeignKey("Tour")]
        public int? TourId { get; set; }
        public Tour? Tour { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TourOrderDate { get; set; }
     
        public int? TourOrderQuantity { get; set; }

        public decimal? TourTotalPrice { get; set; }

        public bool? IsConfirmed { get; set; }
        public bool? Status { get; set; }
    }
}
