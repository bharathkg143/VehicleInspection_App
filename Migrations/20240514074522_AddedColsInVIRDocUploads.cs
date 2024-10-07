using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIR_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedColsInVIRDocUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessName",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_ChassisNo",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_Front90",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_FrontLeft45",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_FrontRight45",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_Left90",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_OdoMeterReading",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_OpenBonnet",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_OpenDickey",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_RCCopyBack",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_RCCopyFront",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_Rear90",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_RearLeft45",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_RearRight45",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_Right90",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadDateTime_UnderBodyFromFrontSide",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessName",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_ChassisNo",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_Front90",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_FrontLeft45",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_FrontRight45",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_Left90",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_OdoMeterReading",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_OpenBonnet",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_OpenDickey",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_RCCopyBack",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_RCCopyFront",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_Rear90",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_RearLeft45",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_RearRight45",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_Right90",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "UploadDateTime_UnderBodyFromFrontSide",
                table: "vir_documents_uploads");
        }
    }
}
