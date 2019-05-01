using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class hvacexpose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_RmsList_RmsConfigRmsId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Hvac_RmsList_RmsConfigRmsId",
                table: "Hvac");

            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Hvac_HvacRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Devices_LightRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hvac",
                table: "Hvac");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "Hvac",
                newName: "Hvacs");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "Lights");

            migrationBuilder.RenameIndex(
                name: "IX_Hvac_RmsConfigRmsId",
                table: "Hvacs",
                newName: "IX_Hvacs_RmsConfigRmsId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_RmsConfigRmsId",
                table: "Lights",
                newName: "IX_Lights_RmsConfigRmsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hvacs",
                table: "Hvacs",
                column: "RmsDeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lights",
                table: "Lights",
                column: "RmsDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hvacs_RmsList_RmsConfigRmsId",
                table: "Hvacs",
                column: "RmsConfigRmsId",
                principalTable: "RmsList",
                principalColumn: "RmsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lights_RmsList_RmsConfigRmsId",
                table: "Lights",
                column: "RmsConfigRmsId",
                principalTable: "RmsList",
                principalColumn: "RmsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebHooks_Hvacs_HvacRmsDeviceId",
                table: "WebHooks",
                column: "HvacRmsDeviceId",
                principalTable: "Hvacs",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebHooks_Lights_LightRmsDeviceId",
                table: "WebHooks",
                column: "LightRmsDeviceId",
                principalTable: "Lights",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hvacs_RmsList_RmsConfigRmsId",
                table: "Hvacs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lights_RmsList_RmsConfigRmsId",
                table: "Lights");

            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Hvacs_HvacRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Lights_LightRmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lights",
                table: "Lights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hvacs",
                table: "Hvacs");

            migrationBuilder.RenameTable(
                name: "Lights",
                newName: "Devices");

            migrationBuilder.RenameTable(
                name: "Hvacs",
                newName: "Hvac");

            migrationBuilder.RenameIndex(
                name: "IX_Lights_RmsConfigRmsId",
                table: "Devices",
                newName: "IX_Devices_RmsConfigRmsId");

            migrationBuilder.RenameIndex(
                name: "IX_Hvacs_RmsConfigRmsId",
                table: "Hvac",
                newName: "IX_Hvac_RmsConfigRmsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "RmsDeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hvac",
                table: "Hvac",
                column: "RmsDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_RmsList_RmsConfigRmsId",
                table: "Devices",
                column: "RmsConfigRmsId",
                principalTable: "RmsList",
                principalColumn: "RmsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hvac_RmsList_RmsConfigRmsId",
                table: "Hvac",
                column: "RmsConfigRmsId",
                principalTable: "RmsList",
                principalColumn: "RmsId",
                onDelete: ReferentialAction.Restrict);

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
    }
}
