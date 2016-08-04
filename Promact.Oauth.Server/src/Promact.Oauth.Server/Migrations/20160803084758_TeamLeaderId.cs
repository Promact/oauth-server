using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Oauth.Server.Migrations
{
    public partial class TeamLeaderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TeamLeaderId",
                table: "Projects",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeamLeaderId",
                table: "Projects",
                nullable: false);
        }
    }
}
