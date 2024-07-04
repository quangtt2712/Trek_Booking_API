using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Trek_Booking_DataAccess;

public class OrderTourHeader
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int? UserId { get; set; }

    public User? User { get; set; }

    public int? SupplierId { get; set; }


    public decimal? TotalPrice { get; set; }


    [DataType(DataType.DateTime)]
    public DateTime? TourOrderDate { get; set; }

    public string? SessionId { get; set; }

    public string? PaymentIntentId { get; set; }


    public string? FullName { get; set; }


    public string? Email { get; set; }


    public string? Phone { get; set; }

    public string? Process { get; set; }

    public bool? Completed { get; set; }


}