using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMicroHabitsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoalEnergyPercent = table.Column<double>(type: "float", nullable: false),
                    PrivateGasPercent = table.Column<double>(type: "float", nullable: false),
                    PublicDieselPercent = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HousingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightsPerYear = table.Column<int>(type: "int", nullable: false),
                    HeatingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcUsageMonths = table.Column<int>(type: "int", nullable: false),
                    HasWaterMeter = table.Column<bool>(type: "bit", nullable: false),
                    HasLedLights = table.Column<bool>(type: "bit", nullable: false),
                    ShoppingFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasRecycling = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeCupsPerMonth = table.Column<int>(type: "int", nullable: false),
                    ShoppingTripsPerWeek = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<double>(type: "float", nullable: false),
                    MobilityScore = table.Column<double>(type: "float", nullable: false),
                    LivingScore = table.Column<double>(type: "float", nullable: false),
                    ConsumptionScore = table.Column<double>(type: "float", nullable: false),
                    Co2Trees = table.Column<int>(type: "int", nullable: false),
                    WaterPools = table.Column<double>(type: "float", nullable: false),
                    SystemTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnualCoffeePlasticGrams = table.Column<double>(type: "float", nullable: false),
                    AnnualBagPlasticGrams = table.Column<double>(type: "float", nullable: false),
                    TotalPlasticKg = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResults", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CoalEnergyPercent", "Name", "PrivateGasPercent", "PublicDieselPercent", "Region" },
                values: new object[,]
                {
                    { 1, 55.0, "Алматы қ.", 15.0, 10.0, "Мегаполис" },
                    { 2, 87.0, "Астана қ.", 6.0, 45.0, "Мегаполис" },
                    { 3, 25.0, "Шымкент қ.", 47.0, 5.0, "Мегаполис" },
                    { 4, 97.0, "Қарағанды", 5.0, 85.0, "Қарағанды облысы" },
                    { 5, 99.0, "Павлодар", 4.0, 82.0, "Павлодар облысы" },
                    { 6, 65.0, "Өскемен", 3.0, 90.0, "Шығыс Қазақстан облысы" },
                    { 7, 67.0, "Семей", 4.0, 88.0, "Абай облысы" },
                    { 8, 48.0, "Талдықорған", 20.0, 75.0, "Жетісу облысы" },
                    { 9, 92.0, "Жезқазған", 5.0, 90.0, "Ұлытау облысы" },
                    { 10, 30.0, "Тараз", 35.0, 40.0, "Жамбыл облысы" },
                    { 11, 20.0, "Қызылорда", 43.0, 10.0, "Қызылорда облысы" },
                    { 12, 27.0, "Түркістан", 45.0, 35.0, "Түркістан облысы" },
                    { 13, 0.0, "Орал", 40.0, 30.0, "Батыс Қазақстан облысы" },
                    { 14, 0.0, "Ақтөбе", 43.0, 15.0, "Ақтөбе облысы" },
                    { 15, 0.0, "Атырау", 50.0, 25.0, "Атырау облысы" },
                    { 16, 0.0, "Ақтау", 79.0, 20.0, "Маңғыстау облысы" },
                    { 17, 87.0, "Қостанай", 5.0, 78.0, "Қостанай облысы" },
                    { 18, 95.0, "Петропавл", 3.0, 92.0, "Солтүстік Қазақстан облысы" },
                    { 19, 97.0, "Көкшетау", 4.0, 85.0, "Ақмола облысы" },
                    { 20, 57.0, "Қонаев", 22.0, 65.0, "Алматы облысы" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "SurveyResults");
        }
    }
}
