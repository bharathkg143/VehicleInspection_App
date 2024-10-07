using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace VIR_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customer_info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: true),
                    vehicleRegNo = table.Column<string>(type: "longtext", nullable: true),
                    customerName = table.Column<string>(type: "longtext", nullable: true),
                    customer_id = table.Column<long>(type: "bigint", nullable: true),
                    chassisNo = table.Column<string>(type: "longtext", nullable: true),
                    engineNo = table.Column<string>(type: "longtext", nullable: true),
                    phoneNumber = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_info", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "generated_otp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OTP = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generated_otp", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sms_interaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    MobileNumber = table.Column<long>(type: "bigint", nullable: true),
                    Message = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true),
                    MsgType = table.Column<string>(type: "longtext", nullable: true),
                    SmsType = table.Column<string>(type: "longtext", nullable: true),
                    MsgStatus = table.Column<string>(type: "longtext", nullable: true),
                    ResponseFromGateway = table.Column<string>(type: "longtext", nullable: true),
                    InteractionDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms_interaction", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sms_template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SmsApi = table.Column<string>(type: "longtext", nullable: true),
                    SmsTemplate = table.Column<string>(type: "longtext", nullable: true),
                    SmsType = table.Column<string>(type: "longtext", nullable: true),
                    DealerName = table.Column<string>(type: "longtext", nullable: true),
                    IsActive = table.Column<ulong>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms_template", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vir_documents_uploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    VehicleRegNo = table.Column<string>(type: "longtext", nullable: true),
                    ChassisNo = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_Left90 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_Right90 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_Front90 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_Rear90 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_FrontLeft45 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_FrontRight45 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_RearLeft45 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_RearRight45 = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_OpenBonnet = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_OpenDickey = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_UnderBodyFromFrontSide = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_ChassisNo = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_RCCopyFront = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_RCCopyBack = table.Column<string>(type: "longtext", nullable: true),
                    ImgUrl_OdoMeterReading = table.Column<string>(type: "longtext", nullable: true),
                    LastUploadDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vir_documents_uploads", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_info");

            migrationBuilder.DropTable(
                name: "generated_otp");

            migrationBuilder.DropTable(
                name: "sms_interaction");

            migrationBuilder.DropTable(
                name: "sms_template");

            migrationBuilder.DropTable(
                name: "vir_documents_uploads");
        }
    }
}
