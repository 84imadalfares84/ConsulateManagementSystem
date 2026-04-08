using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Consulate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class revoke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a035f707-eed9-4b20-b8f7-4017ad5030e5"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c635b99c-3cc6-4900-ab7e-c31f44fb9a10"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1e580ac9-ae4c-43f3-8e65-3808e275b71d"), new DateTime(2026, 4, 8, 11, 20, 41, 423, DateTimeKind.Utc).AddTicks(6259), false, "Admin", null },
                    { new Guid("2c295bbd-fd14-499e-979c-7edc76753686"), new DateTime(2026, 4, 8, 11, 20, 41, 423, DateTimeKind.Utc).AddTicks(6266), false, "Officer", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1e580ac9-ae4c-43f3-8e65-3808e275b71d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2c295bbd-fd14-499e-979c-7edc76753686"));

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "RefreshTokens");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a035f707-eed9-4b20-b8f7-4017ad5030e5"), new DateTime(2026, 3, 30, 10, 49, 0, 8, DateTimeKind.Utc).AddTicks(1651), false, "Officer", null },
                    { new Guid("c635b99c-3cc6-4900-ab7e-c31f44fb9a10"), new DateTime(2026, 3, 30, 10, 49, 0, 8, DateTimeKind.Utc).AddTicks(1645), false, "Admin", null }
                });
        }
    }
}
