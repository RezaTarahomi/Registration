using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Registration.Migrations
{
    public partial class addlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dae7c9ad-0981-4389-9b7d-68c86d26076d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef46d212-f17d-4475-bd7b-483fc809f71c");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    machinename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    logger = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7834f975-3642-4266-b2c6-53640184226d", "4d4c4b06-ac73-4390-9c1f-ce61f551c619", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba29ca6a-2a72-40d8-930f-39d18b078063", "ccdfcb85-3ea1-4cdf-95ea-c70fed05fc70", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7834f975-3642-4266-b2c6-53640184226d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba29ca6a-2a72-40d8-930f-39d18b078063");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dae7c9ad-0981-4389-9b7d-68c86d26076d", "abcb1579-ba7c-4c1f-9585-066c36a77cb8", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ef46d212-f17d-4475-bd7b-483fc809f71c", "b2bda2b2-80d4-4a07-b37a-210b79ca80a1", "Administrator", "ADMINISTRATOR" });
        }
    }
}
