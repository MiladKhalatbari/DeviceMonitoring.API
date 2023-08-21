using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeviceMonitoring.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigBuildDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(type: "float", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "InstallationDate", "IsActive", "Location", "Manufacturer", "ModelNumber", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Location A", "Manufacturer X", "Model 123", "Device 1" },
                    { 2, new DateTime(2021, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Location B", "Manufacturer Y", "Model 456", "Device 2" },
                    { 3, new DateTime(2020, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Location C", "Manufacturer Z", "Model 789", "Device 3" },
                    { 4, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Location D", "Manufacturer X", "Model 234", "Device 4" },
                    { 5, new DateTime(2023, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Location E", "Manufacturer Z", "Model 567", "Device 5" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "Password", "UserName" },
                values: new object[] { 1, "Milad", "Khalatbari", "123456", "Admin" });

            migrationBuilder.InsertData(
                table: "Measurements",
                columns: new[] { "Id", "CreatedOn", "DeviceId", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 1, 10.5 },
                    { 2, new DateTime(2023, 8, 21, 1, 0, 0, 0, DateTimeKind.Unspecified), 2, 15.199999999999999 },
                    { 3, new DateTime(2023, 8, 20, 6, 0, 0, 0, DateTimeKind.Unspecified), 3, 9.8000000000000007 },
                    { 4, new DateTime(2023, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), 4, 12.699999999999999 },
                    { 5, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 5, 11.300000000000001 },
                    { 6, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 1, 14.9 },
                    { 7, new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 2, 8.5999999999999996 },
                    { 8, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 3, 13.1 },
                    { 9, new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 4, 10.0 },
                    { 10, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 5, 16.399999999999999 },
                    { 11, new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 1, 9.1999999999999993 },
                    { 12, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 2, 11.699999999999999 },
                    { 13, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 3, 14.1 },
                    { 14, new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 4, 7.7999999999999998 },
                    { 15, new DateTime(2023, 8, 20, 20, 0, 0, 0, DateTimeKind.Unspecified), 5, 10.300000000000001 },
                    { 16, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 1, 12.9 },
                    { 17, new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, 15.6 },
                    { 18, new DateTime(2023, 8, 21, 6, 0, 0, 0, DateTimeKind.Unspecified), 3, 8.0999999999999996 },
                    { 19, new DateTime(2023, 8, 21, 11, 0, 0, 0, DateTimeKind.Unspecified), 4, 11.5 },
                    { 20, new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 5, 13.699999999999999 },
                    { 21, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 2, 15.199999999999999 },
                    { 22, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 3, 9.8000000000000007 },
                    { 23, new DateTime(2023, 8, 21, 6, 0, 0, 0, DateTimeKind.Unspecified), 4, 12.699999999999999 },
                    { 24, new DateTime(2023, 8, 21, 4, 0, 0, 0, DateTimeKind.Unspecified), 5, 11.300000000000001 },
                    { 25, new DateTime(2023, 8, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), 1, 14.9 },
                    { 26, new DateTime(2023, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), 2, 8.5999999999999996 },
                    { 27, new DateTime(2023, 8, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, 13.1 },
                    { 28, new DateTime(2023, 8, 20, 6, 0, 0, 0, DateTimeKind.Unspecified), 4, 10.0 },
                    { 29, new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 5, 16.399999999999999 },
                    { 30, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 3, 16.399999999999999 },
                    { 31, new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 2, 18.699999999999999 },
                    { 32, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 2, 22.300000000000001 },
                    { 33, new DateTime(2023, 8, 20, 2, 0, 0, 0, DateTimeKind.Unspecified), 2, 14.800000000000001 },
                    { 34, new DateTime(2023, 8, 20, 19, 0, 0, 0, DateTimeKind.Unspecified), 3, 9.5 },
                    { 35, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.199999999999999 },
                    { 36, new DateTime(2023, 8, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, 15.9 },
                    { 37, new DateTime(2023, 8, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), 4, 7.2999999999999998 },
                    { 38, new DateTime(2023, 8, 20, 5, 0, 0, 0, DateTimeKind.Unspecified), 4, 11.1 },
                    { 39, new DateTime(2023, 8, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), 4, 16.5 },
                    { 40, new DateTime(2023, 8, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 5, 8.8000000000000007 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name",
                table: "Devices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_DeviceId",
                table: "Measurements",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
