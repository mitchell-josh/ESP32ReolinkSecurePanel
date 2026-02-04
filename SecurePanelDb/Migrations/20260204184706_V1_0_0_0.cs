using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecurePanelDb.Migrations
{
    /// <inheritdoc />
    public partial class V1_0_0_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlarmChannels",
                columns: table => new
                {
                    AlarmChannelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Identifier = table.Column<int>(type: "INTEGER", nullable: false),
                    Online = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmChannels", x => x.AlarmChannelId);
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchedules",
                columns: table => new
                {
                    AlarmScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PetsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    OtherEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PeopleEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    VehicleEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchedules", x => x.AlarmScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemeTypes",
                columns: table => new
                {
                    AlarmSchemeTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchemeTypes", x => x.AlarmSchemeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AlarmUsers",
                columns: table => new
                {
                    AlarmUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    AlarmCodeHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmUsers", x => x.AlarmUserId);
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemes",
                columns: table => new
                {
                    AlarmSchemeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlarmChannelId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlarmSchemeTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlarmScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PushEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchemes", x => x.AlarmSchemeId);
                    table.ForeignKey(
                        name: "FK_AlarmSchemes_AlarmChannels_AlarmChannelId",
                        column: x => x.AlarmChannelId,
                        principalTable: "AlarmChannels",
                        principalColumn: "AlarmChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlarmSchemes_AlarmSchedules_AlarmScheduleId",
                        column: x => x.AlarmScheduleId,
                        principalTable: "AlarmSchedules",
                        principalColumn: "AlarmScheduleId");
                    table.ForeignKey(
                        name: "FK_AlarmSchemes_AlarmSchemeTypes_AlarmSchemeTypeId",
                        column: x => x.AlarmSchemeTypeId,
                        principalTable: "AlarmSchemeTypes",
                        principalColumn: "AlarmSchemeTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemes_AlarmChannelId",
                table: "AlarmSchemes",
                column: "AlarmChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemes_AlarmScheduleId",
                table: "AlarmSchemes",
                column: "AlarmScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemes_AlarmSchemeTypeId",
                table: "AlarmSchemes",
                column: "AlarmSchemeTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmSchemes");

            migrationBuilder.DropTable(
                name: "AlarmUsers");

            migrationBuilder.DropTable(
                name: "AlarmChannels");

            migrationBuilder.DropTable(
                name: "AlarmSchedules");

            migrationBuilder.DropTable(
                name: "AlarmSchemeTypes");
        }
    }
}
