using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class rmsdeviceadjust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentStatus",
                table: "Devices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MAC",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentStatus",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "MAC",
                table: "Devices");
        }
    }
}
