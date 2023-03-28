using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev_Events_App.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DevEventsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DevEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Start_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevEventsSpeakers",
                columns: table => new
                {
                    DevEventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    TalkTitle = table.Column<string>(type: "TEXT", nullable: true),
                    TalkDescription = table.Column<string>(type: "TEXT", nullable: true),
                    LinkedInProfile = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevEventsSpeakers", x => x.DevEventId);
                    table.ForeignKey(
                        name: "FK_DevEventsSpeakers_DevEvents_DevEventId",
                        column: x => x.DevEventId,
                        principalTable: "DevEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevEventsSpeakers");

            migrationBuilder.DropTable(
                name: "DevEvents");
        }
    }
}
