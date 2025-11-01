using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests");

            migrationBuilder.DropIndex(
                name: "IX_QuotationRequests_CustomerId",
                table: "QuotationRequests");

            migrationBuilder.RenameColumn(
                name: "OfficerMessage",
                table: "QuotationRequests",
                newName: "CustomerName");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "QuotationRequests",
                type: "TEXT",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "ContainerType",
                table: "QuotationRequests",
                type: "TEXT",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "QuotationRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "QuotationRequests",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationRequests_CustomerId_Status",
                table: "QuotationRequests",
                columns: new[] { "CustomerId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuotationRequests_CustomerId_Status",
                table: "QuotationRequests");

            migrationBuilder.DropColumn(
                name: "ContainerType",
                table: "QuotationRequests");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "QuotationRequests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "QuotationRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "QuotationRequests",
                newName: "OfficerMessage");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "QuotationRequests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 16);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationRequests_CustomerId",
                table: "QuotationRequests",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
