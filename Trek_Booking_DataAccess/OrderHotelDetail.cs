using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Trek_Booking_DataAccess;

[Table("OrderHotelDetail")]
public class OrderHotelDetail
{
    [Key]
    public int Id { get; set; }

    public int? OrderHotelHeaderlId { get; set; }

    [ForeignKey("Hotel")]
    public int? HotelId { get; set; }
    public virtual Hotel? Hotel { get; set; }

    [ForeignKey("Room")]
    public int? RoomId { get; set; }
    public virtual Room? Room { get; set; }
    public string? RoomName { get; set; }

    public string? HotelName { get; set; }


    public double? Price { get; set; }
    public int? RoomQuantity { get; set; }
}
