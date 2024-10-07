using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIR_WebApp.Models
{
    [Table("sms_template")]
    public class SMSTemplates
    {
        [Key]
        public int Id { get; set; }
        public string? SmsApi { get; set; }
        public string? SmsTemplate { get; set; }
        public string? SmsType { get; set; }
        public string? ServiceType { get; set; }
        public string? DealerName { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsActive { get; set; }
        public string? TemplateId { get; set; }
    }
}
