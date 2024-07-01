using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("TourImage")]
    public class TourImage
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TourImageId { get; set; }
        [Required(ErrorMessage ="TourImage is not null")]
        public string? TourImageURL { get; set; }
        [ForeignKey("Tour")]
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
    }
}
