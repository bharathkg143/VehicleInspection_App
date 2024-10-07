using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIR_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddColLongandLat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "vir_documents_uploads");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "vir_documents_uploads");
        }
    }
}
