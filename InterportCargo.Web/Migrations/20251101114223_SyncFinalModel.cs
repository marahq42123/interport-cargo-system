using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Web.Migrations
{
    /// <inheritdoc />
    public partial class SyncFinalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "QuotationRequests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 16);

            migrationBuilder.AddColumn<string>(
                name: "OfficerMessage",
                table: "QuotationRequests",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuotationRequests_Customers_CustomerId",
                table: "QuotationRequests");

            migrationBuilder.DropColumn(
                name: "OfficerMessage",
                table: "QuotationRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "QuotationRequests",
                type: "TEXT",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
