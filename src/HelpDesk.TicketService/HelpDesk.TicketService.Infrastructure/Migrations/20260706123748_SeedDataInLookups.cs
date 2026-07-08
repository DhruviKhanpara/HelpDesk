using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HelpDesk.TicketService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataInLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "lookup",
                table: "Categories",
                columns: new[] { "Id", "CategoryCode", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1L, "HW", "Hardware related issues", 1, true, "Hardware" },
                    { 2L, "SW", "Software related issues", 2, true, "Software" },
                    { 3L, "NET", "Network and connectivity issues", 3, true, "Network" },
                    { 4L, "EMAIL", "Email related issues", 4, true, "Email" },
                    { 5L, "PRN", "Printer issues", 5, true, "Printer" },
                    { 6L, "ACC", "Access and permission requests", 6, true, "Access" },
                    { 7L, "OTH", "Miscellaneous requests", 7, true, "Other" }
                });

            migrationBuilder.InsertData(
                schema: "lookup",
                table: "Priorities",
                columns: new[] { "Id", "Color", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1L, "#28A745", "Low priority", 1, true, "Low" },
                    { 2L, "#FFC107", "Medium priority", 2, true, "Medium" },
                    { 3L, "#FD7E14", "High priority", 3, true, "High" },
                    { 4L, "#DC3545", "Critical priority", 4, true, "Critical" }
                });

            migrationBuilder.InsertData(
                schema: "lookup",
                table: "Statuses",
                columns: new[] { "Id", "Color", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1L, "#0D6EFD", null, 1, true, "Open" },
                    { 2L, "#6610F2", null, 2, true, "Assigned" },
                    { 3L, "#0DCAF0", null, 3, true, "In Progress" },
                    { 4L, "#FFC107", null, 4, true, "Waiting for User" },
                    { 5L, "#198754", null, 5, true, "Resolved" }
                });

            migrationBuilder.InsertData(
                schema: "lookup",
                table: "Statuses",
                columns: new[] { "Id", "Color", "Description", "DisplayOrder", "IsActive", "IsClosedStatus", "Name" },
                values: new object[,]
                {
                    { 6L, "#6C757D", null, 6, true, true, "Closed" },
                    { 7L, "#DC3545", null, 7, true, true, "Cancelled" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 7L);
        }
    }
}
