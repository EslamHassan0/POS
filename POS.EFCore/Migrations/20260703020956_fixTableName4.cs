using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class fixTableName4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Items_ItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Items_ItemId",
                table: "PurchaseOrderItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Items_ItemId",
                table: "PurchaseOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Items_ItemId",
                table: "PurchaseOrderItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
