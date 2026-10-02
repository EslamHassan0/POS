using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class fixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Items_ItemId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemCategories_ItemCategoryId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Warehouses",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Suppliers",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "StockTransactions",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "PurchaseOrders",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "PurchaseOrderItems",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "POSSettings",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Payments",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "PaymentMethod",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "ItemCategoryId",
                table: "Items",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Items",
                newName: "CreationTime");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ItemCategoryId",
                table: "Items",
                newName: "IX_Items_CategoryId");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Invoices",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "InvoiceItems",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Customers",
                newName: "CreationTime");

            migrationBuilder.AlterColumn<decimal>(
                name: "VatPercent",
                table: "Items",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Barcode",
                table: "Items",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Code",
                table: "Items",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Items_ItemId",
                table: "InvoiceItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Items_ItemId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Items_Barcode",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_Code",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Warehouses",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Suppliers",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "StockTransactions",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "PurchaseOrders",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "PurchaseOrderItems",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "POSSettings",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Payments",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "PaymentMethod",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Items",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Items",
                newName: "ItemCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                newName: "IX_Items_ItemCategoryId");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Invoices",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "InvoiceItems",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Customers",
                newName: "CreateAt");

            migrationBuilder.AlterColumn<decimal>(
                name: "VatPercent",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Items",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Items_ItemId",
                table: "InvoiceItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemCategories_ItemCategoryId",
                table: "Items",
                column: "ItemCategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
