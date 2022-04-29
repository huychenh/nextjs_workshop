using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStore.Authentication.Migrations
{
    public partial class UpdateCode220429 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "asdfghjklmnbvcxzqwertyuio01",
                column: "ConcurrencyStamp",
                value: "05704ff3-7cee-4266-ba42-f686d56c2bf8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "asdfghjklmnbvcxzqwertyuio02",
                column: "ConcurrencyStamp",
                value: "22ef1600-1904-46ed-9567-8185851d23d2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "asdfghjklmnbvcxzqwertyuio01",
                column: "ConcurrencyStamp",
                value: "c8585528-9221-4dd3-a0dd-5c617b6070ba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "asdfghjklmnbvcxzqwertyuio02",
                column: "ConcurrencyStamp",
                value: "2f609245-5cd2-4422-8153-9a780a7fce0d");
        }
    }
}
