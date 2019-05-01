using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class rmsdevicesplit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Devices_RmsDeviceId",
                table: "WebHooks");

            migrationBuilder.RenameColumn(
                name: "RmsDeviceId",
                table: "WebHooks",
                newName: "LightRmsDeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_WebHooks_RmsDeviceId",
                table: "WebHooks",
                newName: "IX_WebHooks_LightRmsDeviceId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "RmsList",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "HvacRmsDeviceId",
                table: "WebHooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "RmsList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "RmsList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "RmsList",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Hvac",
                columns: table => new
                {
                    RmsDeviceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MAC = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    CurrentTemperature = table.Column<double>(nullable: false),
                    SetpointTemperature = table.Column<double>(nullable: false),
                    RmsConfigRmsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hvac", x => x.RmsDeviceId);
                    table.ForeignKey(
                        name: "FK_Hvac_RmsList_RmsConfigRmsId",
                        column: x => x.RmsConfigRmsId,
                        principalTable: "RmsList",
                        principalColumn: "RmsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebHooks_HvacRmsDeviceId",
                table: "WebHooks",
                column: "HvacRmsDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Hvac_RmsConfigRmsId",
                table: "Hvac",
                column: "RmsConfigRmsId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebHooks_Hvac_HvacRmsDeviceId",
                table: "WebHooks",
                column: "HvacRmsDeviceId",
                principalTable: "Hvac",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebHooks_Devices_LightRmsDeviceId",
                table: "WebHooks",
                column: "LightRmsDeviceId",
                principalTable: "Devices",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Hvac_HvacRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Devices_LightRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropTable(
                name: "Hvac");

            migrationBuilder.DropIndex(
                name: "IX_WebHooks_HvacRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropColumn(
                name: "HvacRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "RmsList");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "RmsList");

            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "RmsList");

            migrationBuilder.RenameColumn(
                name: "LightRmsDeviceId",
                table: "WebHooks",
                newName: "RmsDeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_WebHooks_LightRmsDeviceId",
                table: "WebHooks",
                newName: "IX_WebHooks_RmsDeviceId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "RmsList",
                newName: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_WebHooks_Devices_RmsDeviceId",
                table: "WebHooks",
                column: "RmsDeviceId",
                principalTable: "Devices",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
