using Microsoft.EntityFrameworkCore.Migrations;

namespace Procurement.Api.Migrations
{
    public partial class quote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotation_Requisitions_RequisitionId",
                table: "Quotation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotation",
                table: "Quotation");

            migrationBuilder.RenameTable(
                name: "Quotation",
                newName: "Quotations");

            migrationBuilder.RenameIndex(
                name: "IX_Quotation_RequisitionId",
                table: "Quotations",
                newName: "IX_Quotations_RequisitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotations",
                table: "Quotations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_Requisitions_RequisitionId",
                table: "Quotations",
                column: "RequisitionId",
                principalTable: "Requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_Requisitions_RequisitionId",
                table: "Quotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotations",
                table: "Quotations");

            migrationBuilder.RenameTable(
                name: "Quotations",
                newName: "Quotation");

            migrationBuilder.RenameIndex(
                name: "IX_Quotations_RequisitionId",
                table: "Quotation",
                newName: "IX_Quotation_RequisitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotation",
                table: "Quotation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotation_Requisitions_RequisitionId",
                table: "Quotation",
                column: "RequisitionId",
                principalTable: "Requisitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
