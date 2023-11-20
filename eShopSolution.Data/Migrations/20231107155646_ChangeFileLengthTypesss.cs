using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShopSolution.Data.Migrations
{
    public partial class ChangeFileLengthTypesss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "5f8358dd-27ae-4f87-9210-5ec84232ccc7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fb2ec8dd-342b-473e-be7d-c6df54b8e4ef", "AQAAAAEAACcQAAAAENmkhlwu6Ec4a/b8GKfRKSlCepQErU4FT/3g/3LMD72C5wCgmKgsanga27hsyP3CLg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 7, 20, 26, 45, 406, DateTimeKind.Local).AddTicks(4466));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "326eeaf4-be37-408c-808f-626b1f75a749");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "18b721d5-9eba-498f-a76f-d78e4cf7667c", "AQAAAAEAACcQAAAAEKyOLbf08tF6sH+UbLWTrdRmsRr92jznUTAVRLji5Z9GxWmrE7eogEXiIPfozcF4Ng==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 7, 20, 24, 16, 324, DateTimeKind.Local).AddTicks(9554));
        }
    }
}
