using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIR_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class DocsUploadedLinkColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocsUploadedLink",
                table: "vir_documents_uploads",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocsUploadedLink",
                table: "vir_documents_uploads");
        }
    }
}
