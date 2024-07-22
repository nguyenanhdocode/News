using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class _00013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 16, 44, 29, 516, DateTimeKind.Utc).AddTicks(2489),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 13, 44, 35, 163, DateTimeKind.Utc).AddTicks(4840));

            migrationBuilder.AddColumn<Guid>(
                name: "CoverPhotoId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 16, 44, 29, 518, DateTimeKind.Utc).AddTicks(1527),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 13, 44, 35, 165, DateTimeKind.Utc).AddTicks(1691));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("a42109cb-8f75-4480-958b-fc71a124c72b"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("78a22dc2-03f4-46cb-85bd-9c081c27243e"));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CoverPhotoId",
                table: "Posts",
                column: "CoverPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Assets_CoverPhotoId",
                table: "Posts",
                column: "CoverPhotoId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Assets_CoverPhotoId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CoverPhotoId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CoverPhotoId",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 13, 44, 35, 163, DateTimeKind.Utc).AddTicks(4840),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 16, 44, 29, 516, DateTimeKind.Utc).AddTicks(2489));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 15, 13, 44, 35, 165, DateTimeKind.Utc).AddTicks(1691),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 15, 16, 44, 29, 518, DateTimeKind.Utc).AddTicks(1527));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Assets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("78a22dc2-03f4-46cb-85bd-9c081c27243e"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("a42109cb-8f75-4480-958b-fc71a124c72b"));
        }
    }
}
