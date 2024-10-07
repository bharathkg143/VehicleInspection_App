using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIR_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddeddTemplateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUploadDateTime",
                table: "vir_documents_uploads",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                table: "sms_template",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "sms_template");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUploadDateTime",
                table: "vir_documents_uploads",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}
