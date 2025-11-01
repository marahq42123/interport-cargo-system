using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InterportCargo.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "RateSchedules");

            migrationBuilder.DropIndex(
                name: "IX_QuotationRequests_CustomerId_Status",
                table: "QuotationRequests");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                table: "Customers");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationRequests_CustomerId",
                table: "QuotationRequests",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuotationRequests_CustomerId",
                table: "QuotationRequests");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeType = table.Column<int>(type: "INTEGER", nullable: false),
                    FamilyName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuotationRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChargesBase = table.Column<decimal>(type: "TEXT", nullable: false),
                    ChargesDepot = table.Column<decimal>(type: "TEXT", nullable: false),
                    ChargesLcl = table.Column<decimal>(type: "TEXT", nullable: false),
                    ContainerType = table.Column<string>(type: "TEXT", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountApplied = table.Column<decimal>(type: "TEXT", nullable: false),
                    QuotationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_QuotationRequests_QuotationRequestId",
                        column: x => x.QuotationRequestId,
                        principalTable: "QuotationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RateSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaseCharge = table.Column<decimal>(type: "TEXT", nullable: false),
                    ContainerType = table.Column<string>(type: "TEXT", nullable: false),
                    DepotCharge = table.Column<decimal>(type: "TEXT", nullable: false),
                    LclDeliveryCharge = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateSchedules", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RateSchedules",
                columns: new[] { "Id", "BaseCharge", "ContainerType", "DepotCharge", "LclDeliveryCharge" },
                values: new object[,]
                {
                    { 1, 1200m, "20GP", 180m, 0m },
                    { 2, 1800m, "40GP", 240m, 0m },
                    { 3, 0m, "LCL", 0m, 250m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationRequests_CustomerId_Status",
                table: "QuotationRequests",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_QuotationRequestId",
                table: "Quotations",
                column: "QuotationRequestId");
        }
    }
}
