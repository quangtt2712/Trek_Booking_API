using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("HotelImage")]
    public class HotelImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HotelImageId { get; set; }

        [Required(ErrorMessage = "HotelImageURL is not null")]
        public string? HotelImageURL { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public Hotel? Hotel { get; set; }
    }
}
