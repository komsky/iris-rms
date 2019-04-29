using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class webhooksupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebHook_Devices_RmsDeviceId",
                table: "WebHook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebHook",
                table: "WebHook");

            migrationBuilder.RenameTable(
                name: "WebHook",
                newName: "WebHooks");

            migrationBuilder.RenameColumn(
                name: "WebHookHookId",
                table: "WebHooks",
                newName: "WebHookId");

            migrationBuilder.RenameIndex(
                name: "IX_WebHook_RmsDeviceId",
                table: "WebHooks",
                newName: "IX_WebHooks_RmsDeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebHooks",
                table: "WebHooks",
                column: "WebHookId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebHooks_Devices_RmsDeviceId",
                table: "WebHooks",
                column: "RmsDeviceId",
                principalTable: "Devices",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebHooks_Devices_RmsDeviceId",
                table: "WebHooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebHooks",
                table: "WebHooks");

            migrationBuilder.RenameTable(
                name: "WebHooks",
                newName: "WebHook");

            migrationBuilder.RenameColumn(
                name: "WebHookId",
                table: "WebHook",
                newName: "WebHookHookId");

            migrationBuilder.RenameIndex(
                name: "IX_WebHooks_RmsDeviceId",
                table: "WebHook",
                newName: "IX_WebHook_RmsDeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebHook",
                table: "WebHook",
                column: "WebHookHookId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebHook_Devices_RmsDeviceId",
                table: "WebHook",
                column: "RmsDeviceId",
                principalTable: "Devices",
                principalColumn: "RmsDeviceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
