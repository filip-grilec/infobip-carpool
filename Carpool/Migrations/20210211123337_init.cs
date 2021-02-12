using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Carpool.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    CarType = table.Column<string>(nullable: false),
                    Color = table.Column<string>(nullable: false),
                    LicencePlate = table.Column<string>(nullable: false),
                    Seats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeName = table.Column<string>(nullable: false),
                    HasDriverLicence = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "TravelPlans",
                columns: table => new
                {
                    TravelPlanId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTimeUtc = table.Column<DateTime>(nullable: false),
                    EndTimeUtc = table.Column<DateTime>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    StartLocationId = table.Column<int>(nullable: false),
                    EndLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPlans", x => x.TravelPlanId);
                    table.ForeignKey(
                        name: "FK_TravelPlans_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelPlans_Locations_EndLocationId",
                        column: x => x.EndLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelPlans_Locations_StartLocationId",
                        column: x => x.StartLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TravelPlanEmployees",
                columns: table => new
                {
                    TravelPlanId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    TravelPlanEmployeesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPlanEmployees", x => new { x.EmployeeId, x.TravelPlanId });
                    table.ForeignKey(
                        name: "FK_TravelPlanEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TravelPlanEmployees_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "TravelPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 1, "Wagon", "red", "ZG 5833-GD", "XC90", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 2, "Convertible", "plum", "ZG 7307-JV", "Jetta", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 3, "Coupe", "grey", "ZG 5423-HM", "Alpine", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 4, "Wagon", "olive", "DA 5832-JD", "Beetle", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 5, "Wagon", "grey", "DA 9590-UF", "Grand Cherokee", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 6, "Cargo Van", "salmon", "KA 2833-EQ", "Malibu", 4 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 7, "Convertible", "ivory", "ST 7364-PN", "Grand Caravan", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 8, "Cargo Van", "gold", "DA 6340-PB", "Durango", 4 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 9, "Minivan", "maroon", "OS 2157-MR", "Model T", 5 });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "CarType", "Color", "LicencePlate", "Name", "Seats" },
                values: new object[] { 10, "Hatchback", "magenta", "ST 3938-EO", "Durango", 5 });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 16, "Matko Škorjanec", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 17, "Dona Šijak", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 18, "Bela Korpar", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 19, "Filip Namjestnik", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 20, "Slavek ĐuračkiĆosić", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 25, "Grga Sprečaković", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 22, "Branimir Vangelovski", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 23, "Tončica Vukadin", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 24, "Ana Trvalovski", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 15, "Dada Lovač", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 21, "Ruža Španić", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 14, "Slavica Rajačić", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 13, "Ema Kuba", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 12, "Mica Magdika", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 1, "Danijela Šabarić", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 3, "Alma Šelja", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 4, "Zdeslava Rauš", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 5, "Lena Valcer", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 6, "Rajna VesanovićDvornik", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 2, "Davor TkalčićDulić", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 8, "Miranda Avramoski", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 9, "Gordana Poplaša", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 10, "Dorana Maroš", true });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 11, "Bruno Macko", false });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "EmployeeName", "HasDriverLicence" },
                values: new object[] { 7, "Staša Celjak", true });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 9, "Supetar" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 1, "Kutina" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 2, "Duga Resa" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 3, "Slavonski Brod" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 4, "Donja Stubica" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 5, "Glina" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 6, "Daruvar" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 7, "Opatija" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 8, "Donja Stubica" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[] { 10, "Vrbovsko" });

            migrationBuilder.CreateIndex(
                name: "IX_TravelPlanEmployees_TravelPlanId",
                table: "TravelPlanEmployees",
                column: "TravelPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPlans_CarId",
                table: "TravelPlans",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPlans_EndLocationId",
                table: "TravelPlans",
                column: "EndLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPlans_StartLocationId",
                table: "TravelPlans",
                column: "StartLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelPlanEmployees");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TravelPlans");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
