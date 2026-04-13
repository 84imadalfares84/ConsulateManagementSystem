using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Consulate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class emailverification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1e580ac9-ae4c-43f3-8e65-3808e275b71d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2c295bbd-fd14-499e-979c-7edc76753686"));

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0e19be30-0c59-41d9-905c-5398e20d3b9d"), new DateTime(2026, 4, 13, 10, 13, 29, 748, DateTimeKind.Utc).AddTicks(8920), false, "Admin", null },
                    { new Guid("db2d01c5-983a-40de-bd54-bcecb5619b49"), new DateTime(2026, 4, 13, 10, 13, 29, 748, DateTimeKind.Utc).AddTicks(8926), false, "Officer", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0e19be30-0c59-41d9-905c-5398e20d3b9d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("db2d01c5-983a-40de-bd54-bcecb5619b49"));

            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1e580ac9-ae4c-43f3-8e65-3808e275b71d"), new DateTime(2026, 4, 8, 11, 20, 41, 423, DateTimeKind.Utc).AddTicks(6259), false, "Admin", null },
                    { new Guid("2c295bbd-fd14-499e-979c-7edc76753686"), new DateTime(2026, 4, 8, 11, 20, 41, 423, DateTimeKind.Utc).AddTicks(6266), false, "Officer", null }
                });
        }
    }
}
