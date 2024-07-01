using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trek_Booking_DataAccess
{
    [Table("Service")]
    public class Services
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "ServiceName is not null")]
        public string? ServiceName { get; set; }

        [Required(ErrorMessage = "ServiceDescription is not null")]
        public string? ServiceDescription { get; set; }

        [Required(ErrorMessage = "ServiceImage is not null")]
        public string? ServiceImage { get; set; }
        public ICollection<RoomService>? roomServices { get; set; }    
    }
}
