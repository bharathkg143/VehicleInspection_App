using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIR_WebApp.Models
{
    [Table("vir_documents_uploads")]
    public class VIRDocumentsUploads
    {
        [Key]
        public int Id { get; set; }
        public long? VehicleId { get; set; }
        public long? CustomerId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? VehicleRegNo { get; set; }
        public string? ChassisNo { get; set; }
        public string? DocsUploadedLink { get; set; }
        public string? ProcessName { get; set; }

        public string? ImgUrl_Left90 { get; set; }
        public string? ImgUrl_Right90 { get; set; }
        public string? ImgUrl_Front90 { get; set; }
        public string? ImgUrl_Rear90 { get; set; }
        public string? ImgUrl_FrontLeft45 { get; set; }
        public string? ImgUrl_FrontRight45 { get; set; }
        public string? ImgUrl_RearLeft45 { get; set; }
        public string? ImgUrl_RearRight45 { get; set; }
        public string? ImgUrl_OpenBonnet { get; set; }
        public string? ImgUrl_OpenDickey { get; set; }
        public string? ImgUrl_UnderBodyFromFrontSide { get; set; }
        public string? ImgUrl_ChassisNo { get; set; }
        public string? ImgUrl_RCCopyFront { get; set; }
        public string? ImgUrl_RCCopyBack { get; set; }
        public string? ImgUrl_OdoMeterReading { get; set; }

        public string? UploadDateTime_Left90 { get; set; }
        public string? UploadDateTime_Right90 { get; set; }
        public string? UploadDateTime_Front90 { get; set; }
        public string? UploadDateTime_Rear90 { get; set; }
        public string? UploadDateTime_FrontLeft45 { get; set; }
        public string? UploadDateTime_FrontRight45 { get; set; }
        public string? UploadDateTime_RearLeft45 { get; set; }
        public string? UploadDateTime_RearRight45 { get; set; }
        public string? UploadDateTime_OpenBonnet { get; set; }
        public string? UploadDateTime_OpenDickey { get; set; }
        public string? UploadDateTime_UnderBodyFromFrontSide { get; set; }
        public string? UploadDateTime_ChassisNo { get; set; }
        public string? UploadDateTime_RCCopyFront { get; set; }
        public string? UploadDateTime_RCCopyBack { get; set; }
        public string? UploadDateTime_OdoMeterReading { get; set; }

        public DateTime? LastUploadDateTime {  get; set; }
        public long? OdoMeterReading { get; set; }
        public string? ChassisNoLast6Digits { get; set; }
        public string? EngineNoLast6Digits { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool IsSubmitted { get; set; }

    }
}
