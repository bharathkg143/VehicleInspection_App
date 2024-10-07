using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIR_WebApp.Models
{
    [Table("customer_info")]
    public class CustomerInfo
    {
        [Key]
        public int Id { get; set; }
        public long? vehicle_id { get; set; }
        public string? vehicleRegNo { get; set; }
        public string? customerName { get; set; }
        public long? customer_id { get; set; }
        public string? chassisNo { get; set; }
        public string? engineNo { get; set; }
        public string? phoneNumber { get; set; }
    }
}
