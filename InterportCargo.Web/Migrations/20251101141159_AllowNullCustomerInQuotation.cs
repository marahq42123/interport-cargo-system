using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Web.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullCustomerInQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "QuotationRequests",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "QuotationRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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
