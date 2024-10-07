using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIR_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class FieldsAddedInVIRUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChassisNoLast6Digits",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineNoLast6Digits",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OdoMeterReading",
                table: "vir_documents_uploads",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChassisNoLast6Digits",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "EngineNoLast6Digits",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "OdoMeterReading",
                table: "vir_documents_uploads");
        }
    }
}
