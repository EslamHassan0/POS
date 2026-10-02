using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class editPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderNo",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseOrderNo",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderNo",
                table: "PurchaseOrders",
                column: "PurchaseOrderNo",
                unique: true,
                filter: "[PurchaseOrderNo] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderNo",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseOrderNo",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderNo",
                table: "PurchaseOrders",
                column: "PurchaseOrderNo",
                unique: true);
        }
    }
}
