using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Consulate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c0614f57-717f-4999-b4e3-dd93346a0332"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e333ac00-5394-4f1d-8491-27e5d2549b52"));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0f9e648-491a-4a2c-b3de-ab47b599abb2"), new DateTime(2026, 3, 30, 6, 36, 14, 319, DateTimeKind.Utc).AddTicks(1138), false, "Officer", null },
                    { new Guid("eb3a87d1-cee4-4cf9-a6eb-8dfcecd82c4d"), new DateTime(2026, 3, 30, 6, 36, 14, 319, DateTimeKind.Utc).AddTicks(1131), false, "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c0f9e648-491a-4a2c-b3de-ab47b599abb2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eb3a87d1-cee4-4cf9-a6eb-8dfcecd82c4d"));

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0614f57-717f-4999-b4e3-dd93346a0332"), new DateTime(2026, 3, 25, 7, 54, 17, 729, DateTimeKind.Utc).AddTicks(6327), false, "Officer", null },
                    { new Guid("e333ac00-5394-4f1d-8491-27e5d2549b52"), new DateTime(2026, 3, 25, 7, 54, 17, 729, DateTimeKind.Utc).AddTicks(6316), false, "Admin", null }
                });
        }
    }
}
