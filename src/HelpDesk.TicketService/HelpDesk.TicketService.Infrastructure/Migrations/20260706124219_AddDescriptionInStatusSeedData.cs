using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.TicketService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionInStatusSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Description",
                value: "Ticket has been created and is waiting to be assigned.");

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Description",
                value: "Ticket has been assigned to a support agent.");

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Description",
                value: "Support agent is actively working on the ticket.");

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Description",
                value: "Awaiting additional information or confirmation from the requester.");

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Description",
                value: "Issue has been resolved and is awaiting closure.");

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Description",
                value: "Ticket has been completed and closed.");

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Description",
                value: "Ticket was cancelled and will not be processed.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "lookup",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Description",
                value: null);
        }
    }
}
