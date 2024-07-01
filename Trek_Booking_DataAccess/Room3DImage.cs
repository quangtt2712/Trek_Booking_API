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
    [Table("Room3DImage")]
    public class Room3DImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomImage3DId { get; set; }

        [Required(ErrorMessage = "RoomImage3DURL is not null")]
        public string? RoomImage3DURL { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public Room? Room { get; set; }
    }
}
