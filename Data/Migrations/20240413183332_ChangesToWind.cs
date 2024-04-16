using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToWind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "windSpeed",
                table: "Weather",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "windSpeed",
                table: "Weather",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
