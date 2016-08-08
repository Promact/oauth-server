using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Oauth.Server.Migrations
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_UserId1",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_UserId1",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ProjectUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_UserId",
                table: "ProjectUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_UserId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ProjectUsers",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ProjectUsers",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId1",
                table: "ProjectUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_UserId1",
                table: "ProjectUsers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
