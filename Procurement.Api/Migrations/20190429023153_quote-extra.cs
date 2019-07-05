using Microsoft.EntityFrameworkCore.Migrations;

namespace Procurement.Api.Migrations
{
    public partial class quoteextra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Quotations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedBy",
                table: "Quotations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "Quotations");
        }
    }
}
