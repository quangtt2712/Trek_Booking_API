using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("OrderTourDetail")]
    public class OrderTourDetail
    {
        [Key]
        public int Id { get; set; }
        public int? OrderTourHeaderlId { get; set; }


        [ForeignKey("Tour")]
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        public string? TourName { get; set; }

        public int TourOrderQuantity { get; set; }

        public decimal TourTotalPrice { get; set; }

    }
}
