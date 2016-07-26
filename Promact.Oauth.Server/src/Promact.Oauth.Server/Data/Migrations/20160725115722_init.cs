using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Promact.Oauth.Server.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthId = table.Column<int>(nullable: false),
                    AuthSecret = table.Column<string>(nullable: false),
                    CallbackUrl = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<DateTime>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<DateTime>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    SlackChannelName = table.Column<string>(maxLength: 25, nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<DateTime>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectId",
                table: "ProjectUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
