using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Consulate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0e19be30-0c59-41d9-905c-5398e20d3b9d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("db2d01c5-983a-40de-bd54-bcecb5619b49"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a251f323-f67f-464a-b5d1-11033d46472c"), new DateTime(2026, 5, 10, 8, 3, 42, 964, DateTimeKind.Utc).AddTicks(2445), false, "Admin", null },
                    { new Guid("eea3d43f-4667-4af8-b600-3af916d028d4"), new DateTime(2026, 5, 10, 8, 3, 42, 964, DateTimeKind.Utc).AddTicks(2453), false, "Officer", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a251f323-f67f-464a-b5d1-11033d46472c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eea3d43f-4667-4af8-b600-3af916d028d4"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0e19be30-0c59-41d9-905c-5398e20d3b9d"), new DateTime(2026, 4, 13, 10, 13, 29, 748, DateTimeKind.Utc).AddTicks(8920), false, "Admin", null },
                    { new Guid("db2d01c5-983a-40de-bd54-bcecb5619b49"), new DateTime(2026, 4, 13, 10, 13, 29, 748, DateTimeKind.Utc).AddTicks(8926), false, "Officer", null }
                });
        }
    }
}
