using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class rmsdevicehooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivationCommand",
                table: "WebHook",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Devices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationCommand",
                table: "WebHook");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Devices");
        }
    }
}
