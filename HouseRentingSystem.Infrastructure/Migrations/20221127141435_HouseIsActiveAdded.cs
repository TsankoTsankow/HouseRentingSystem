using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class HouseIsActiveAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
            //    columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
            //    values: new object[] { "c58964b4-697e-40bd-b628-2f17e59192e8", "AQAAAAEAACcQAAAAEFWzQAToqb0IHAEbikQ4tzFMy1ta6/isPCllIzAbOK1Vfk22VviAU5DMVViLZQCvQg==", "8a632843-17d7-4bc3-bb59-290eddfc7577" });

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
            //    columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
            //    values: new object[] { "b544e251-8015-4ee2-ac5e-4e3e4103d719", "AQAAAAEAACcQAAAAEJxBU1La+ML+DxBHZmkbWPC3e6XOqF+hVhwoIXNS2x2VJmNUv7OfVZPPn6WkijKWjg==", "5498f741-77b7-4876-878c-18d92a482128" });

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Houses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76580539-3d78-4f35-befa-ccc29715b11e", "AQAAAAEAACcQAAAAEEeUZfLNow0j+YkyAEPei9ZmF1vnf0S/iTWvRgj89aI9HJgsNPT0mEFhumHqtmCR3w==", "fe64792a-4024-43e7-994c-6c1f11b603a9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a68e6d29-d780-4be9-b8f6-0cee2d88653e", "AQAAAAEAACcQAAAAENy4z8D26rK+XH3Yawa6pddghaOkA5CJoZYqC9XzDDeJGHxwQC96QiAj2ZUzTPjLBw==", "f38f89be-5f70-4f67-a5f6-824b0699ba37" });
        }
    }
}
