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
    [Table("Hotel")]
    public class Hotel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HotelId { get; set; }


        [StringLength(100, ErrorMessage = "The UserName must be greater than 0 and less than or equal 100")]
        public string? HotelName { get; set; }


        [StringLength(50, ErrorMessage = "The UserName must be greater than 0 and less than or equal 50")]
        public string? HotelPhone { get; set; }



        [StringLength(100, ErrorMessage = "The HotelEmail must be greater than 0 and less than or equal 100")]
        public string? HotelEmail { get; set; }


        public string? HotelAvatar { get; set; }


        public string? HotelFulDescription { get; set; }


        public string? HotelDistrict { get; set; }


        [StringLength(200, ErrorMessage = "The HotelCity must be greater than 0 and less than or equal 200 ")]
        public string? HotelCity { get; set; }


        public string? HotelInformation { get; set; }

        public bool IsVerify { get; set; }
        public bool? Lock { get; set; }
        public int HotelView { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        [JsonIgnore]
        public ICollection<Comment>? comments { get; set; }
        [JsonIgnore]
        public ICollection<Rate>? rates { get; set; }
        [JsonIgnore]
        public ICollection<BookingCart>? bookingCarts { get; set; }
        [JsonIgnore]
        public ICollection<Booking>? bookings { get; set; }
        [JsonIgnore]
        public ICollection<Room>? rooms { get; set; }
        [JsonIgnore]
        public ICollection<Voucher>? vouchers { get; set; }

        public ICollection<OrderHotelDetail>? OrderHotelDetails { get; set; }

        public ICollection<HotelImage>? hotelImages { get; set; }

    }
}
