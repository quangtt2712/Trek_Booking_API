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
    [Table("Comment")]
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        [Required(ErrorMessage = "Message is not null")]
        public string? Message { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DateSubmitted { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public Hotel? Hotel { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }

        public int? OrderHotelHeaderId { get; set; }

    }
}
