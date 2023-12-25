using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class ApplicationUserFirstNameAndLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("9f4f2d90-71cf-4fe3-9de2-11d8128c2f7b"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("9fd5c9cd-4b58-4646-a04f-f60414c08055"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("ab6b1e2d-1f83-4e68-af09-ffdfd8da45f3"));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "LastName");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("17a94d6e-c530-4cc6-a9a3-cf183541def4"), "North London, UK (near the border)", new Guid("be38a3d1-5835-4d2d-83fa-ca69dcba5fb8"), 3, new DateTime(2023, 12, 25, 16, 58, 7, 23, DateTimeKind.Utc).AddTicks(1108), "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("85198caa-c7e6-466f-2c80-08dbff165c44"), "Big House Marina" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("370b38cc-7a12-4291-a2cc-086d52fa964e"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("be38a3d1-5835-4d2d-83fa-ca69dcba5fb8"), 2, new DateTime(2023, 12, 25, 16, 58, 7, 23, DateTimeKind.Utc).AddTicks(1123), "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("382618aa-730c-49c6-9935-b88b95b9bb95"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("be38a3d1-5835-4d2d-83fa-ca69dcba5fb8"), 2, new DateTime(2023, 12, 25, 16, 58, 7, 23, DateTimeKind.Utc).AddTicks(1125), "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("17a94d6e-c530-4cc6-a9a3-cf183541def4"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("370b38cc-7a12-4291-a2cc-086d52fa964e"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("382618aa-730c-49c6-9935-b88b95b9bb95"));

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("9f4f2d90-71cf-4fe3-9de2-11d8128c2f7b"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("be38a3d1-5835-4d2d-83fa-ca69dcba5fb8"), 2, new DateTime(2023, 12, 20, 21, 4, 59, 587, DateTimeKind.Utc).AddTicks(3786), "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", false, 1200.00m, null, "Family House Comfort" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("9fd5c9cd-4b58-4646-a04f-f60414c08055"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("be38a3d1-5835-4d2d-83fa-ca69dcba5fb8"), 2, new DateTime(2023, 12, 20, 21, 4, 59, 587, DateTimeKind.Utc).AddTicks(3788), "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", false, 2000.00m, null, "Grand House" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "IsActive", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("ab6b1e2d-1f83-4e68-af09-ffdfd8da45f3"), "North London, UK (near the border)", new Guid("be38a3d1-5835-4d2d-83fa-ca69dcba5fb8"), 3, new DateTime(2023, 12, 20, 21, 4, 59, 587, DateTimeKind.Utc).AddTicks(3772), "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", false, 2100.00m, new Guid("85198caa-c7e6-466f-2c80-08dbff165c44"), "Big House Marina" });
        }
    }
}
