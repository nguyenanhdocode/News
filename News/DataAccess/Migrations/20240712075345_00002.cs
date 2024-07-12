using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class _00002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Users_CreatedUserId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Asset_AvatarId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asset",
                table: "Asset");

            migrationBuilder.RenameTable(
                name: "Asset",
                newName: "Assets");

            migrationBuilder.RenameIndex(
                name: "IX_Asset_CreatedUserId",
                table: "Assets",
                newName: "IX_Assets_CreatedUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 7, 53, 45, 407, DateTimeKind.Utc).AddTicks(2580),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 7, 31, 24, 313, DateTimeKind.Utc).AddTicks(8303));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 7, 53, 45, 408, DateTimeKind.Utc).AddTicks(5230),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 7, 31, 24, 315, DateTimeKind.Utc).AddTicks(2747));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("a7fcfa7e-d7b5-46db-9b33-56d148e49cda"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("0f103181-8243-433f-894e-242d538be81f"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assets",
                table: "Assets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Users_CreatedUserId",
                table: "Assets",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Assets_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Assets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Users_CreatedUserId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Assets_AvatarId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assets",
                table: "Assets");

            migrationBuilder.RenameTable(
                name: "Assets",
                newName: "Asset");

            migrationBuilder.RenameIndex(
                name: "IX_Assets_CreatedUserId",
                table: "Asset",
                newName: "IX_Asset_CreatedUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 7, 31, 24, 313, DateTimeKind.Utc).AddTicks(8303),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 7, 53, 45, 407, DateTimeKind.Utc).AddTicks(2580));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Asset",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 12, 7, 31, 24, 315, DateTimeKind.Utc).AddTicks(2747),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 12, 7, 53, 45, 408, DateTimeKind.Utc).AddTicks(5230));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Asset",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("0f103181-8243-433f-894e-242d538be81f"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("a7fcfa7e-d7b5-46db-9b33-56d148e49cda"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asset",
                table: "Asset",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Users_CreatedUserId",
                table: "Asset",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Asset_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Asset",
                principalColumn: "Id");
        }
    }
}
