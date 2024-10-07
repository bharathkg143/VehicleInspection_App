using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIR_WebApp.Models
{
    [Table("generated_otp")]
    public class GeneratedOTPs
    {
        [Key]
        public int Id { get; set; }
        public long? OTP { get; set; }
        public long? CustomerId { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
