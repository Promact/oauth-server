using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Oauth.Server.Migrations
{
    public partial class AddedUpdatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ProjectUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Apps",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProjectUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Apps",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CallbackUrl",
                table: "Apps",
                maxLength: 255,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "AuthSecret",
                table: "Apps",
                maxLength: 30,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "AuthId",
                table: "Apps",
                maxLength: 15,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Apps");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "ProjectUsers",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Projects",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Apps",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "CallbackUrl",
                table: "Apps",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "AuthSecret",
                table: "Apps",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "AuthId",
                table: "Apps",
                nullable: false);
        }
    }
}
