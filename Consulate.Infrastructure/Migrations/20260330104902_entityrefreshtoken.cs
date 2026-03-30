using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Consulate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entityrefreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c0f9e648-491a-4a2c-b3de-ab47b599abb2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eb3a87d1-cee4-4cf9-a6eb-8dfcecd82c4d"));

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a035f707-eed9-4b20-b8f7-4017ad5030e5"), new DateTime(2026, 3, 30, 10, 49, 0, 8, DateTimeKind.Utc).AddTicks(1651), false, "Officer", null },
                    { new Guid("c635b99c-3cc6-4900-ab7e-c31f44fb9a10"), new DateTime(2026, 3, 30, 10, 49, 0, 8, DateTimeKind.Utc).AddTicks(1645), false, "Admin", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a035f707-eed9-4b20-b8f7-4017ad5030e5"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c635b99c-3cc6-4900-ab7e-c31f44fb9a10"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0f9e648-491a-4a2c-b3de-ab47b599abb2"), new DateTime(2026, 3, 30, 6, 36, 14, 319, DateTimeKind.Utc).AddTicks(1138), false, "Officer", null },
                    { new Guid("eb3a87d1-cee4-4cf9-a6eb-8dfcecd82c4d"), new DateTime(2026, 3, 30, 6, 36, 14, 319, DateTimeKind.Utc).AddTicks(1131), false, "Admin", null }
                });
        }
    }
}
