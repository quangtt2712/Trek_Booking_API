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
    [Table("BookingCart")]
    public class BookingCart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingCartId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }


        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        public Hotel? Hotel { get; set; }



        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public Room? Room { get; set; }



        [DataType(DataType.DateTime)]
        public DateTime? CheckInDate { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? CheckOutDate { get; set; }

        public decimal TotalPrice { get; set; }
        public int RoomQuantity { get; set; }
        public string? VoucherCode { get; set; }
        public string? UserNote { get; set; }

    }
}
