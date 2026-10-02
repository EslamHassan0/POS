using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class editStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceStatue",
                table: "Invoices",
                newName: "InvoiceStatus");

            migrationBuilder.CreateSequence<int>(
                name: "InvoiceNumberSequence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "InvoiceNumberSequence");

            migrationBuilder.RenameColumn(
                name: "InvoiceStatus",
                table: "Invoices",
                newName: "InvoiceStatue");
        }
    }
}
