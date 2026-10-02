using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class fixTableName3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_Date",
                table: "StockTransactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ItemId",
                table: "StockTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ReferenceId",
                table: "StockTransactions",
                column: "ReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_Items_ItemId",
                table: "StockTransactions",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_Items_ItemId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_Date",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ItemId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ReferenceId",
                table: "StockTransactions");
        }
    }
}
