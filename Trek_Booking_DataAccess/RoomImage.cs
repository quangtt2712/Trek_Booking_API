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
    [Table("RoomImage")]
    public class RoomImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomImageId { get; set; }

        [Required(ErrorMessage = "RoomImageURL is not null")]
        public string? RoomImageURL { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public Room? Room { get; set; }
    }
}
