using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.TicketService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuditFieldFromLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_IsDeleted",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Priorities_IsDeleted",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsDeleted",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "lookup",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "lookup",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "lookup",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "lookup",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "lookup",
                table: "Statuses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "lookup",
                table: "Statuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "lookup",
                table: "Statuses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "lookup",
                table: "Statuses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "lookup",
                table: "Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "lookup",
                table: "Statuses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "lookup",
                table: "Statuses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "lookup",
                table: "Priorities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "lookup",
                table: "Priorities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "lookup",
                table: "Priorities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "lookup",
                table: "Priorities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "lookup",
                table: "Priorities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "lookup",
                table: "Priorities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "lookup",
                table: "Priorities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "lookup",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "lookup",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                schema: "lookup",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "lookup",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "lookup",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "lookup",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "lookup",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_IsDeleted",
                schema: "lookup",
                table: "Statuses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Priorities_IsDeleted",
                schema: "lookup",
                table: "Priorities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsDeleted",
                schema: "lookup",
                table: "Categories",
                column: "IsDeleted");
        }
    }
}
