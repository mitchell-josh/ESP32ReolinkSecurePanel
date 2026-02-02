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
                name: "AiSchedules",
                columns: table => new
                {
                    AiScheduleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AiDogCat = table.Column<bool>(type: "INTEGER", nullable: false),
                    AiOther = table.Column<bool>(type: "INTEGER", nullable: false),
                    AiPeople = table.Column<bool>(type: "INTEGER", nullable: false),
                    AiVehicle = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiSchedules", x => x.AiScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemes",
                columns: table => new
                {
                    AlarmSchemeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlarmSchemeTypeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchemes", x => x.AlarmSchemeId);
                    table.ForeignKey(
                        name: "FK_AlarmSchemes_AlarmSchemes_AlarmSchemeTypeId",
                        column: x => x.AlarmSchemeTypeId,
                        principalTable: "AlarmSchemes",
                        principalColumn: "AlarmSchemeId");
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemeTypes",
                columns: table => new
                {
                    AlarmSchemeTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "Audios",
                columns: table => new
                {
                    AudioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AiScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audios", x => x.AudioId);
                    table.ForeignKey(
                        name: "FK_Audios_AiSchedules_AiScheduleId",
                        column: x => x.AiScheduleId,
                        principalTable: "AiSchedules",
                        principalColumn: "AiScheduleId");
                });

            migrationBuilder.CreateTable(
                name: "Buzzers",
                columns: table => new
                {
                    BuzzerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AiScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    Channel = table.Column<int>(type: "INTEGER", nullable: false),
                    DiskErrorAlert = table.Column<bool>(type: "INTEGER", nullable: false),
                    DiskFullAlert = table.Column<bool>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IpConfigAlert = table.Column<bool>(type: "INTEGER", nullable: false),
                    NvrDisconnectAlert = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buzzers", x => x.BuzzerId);
                    table.ForeignKey(
                        name: "FK_Buzzers_AiSchedules_AiScheduleId",
                        column: x => x.AiScheduleId,
                        principalTable: "AiSchedules",
                        principalColumn: "AiScheduleId");
                });

            migrationBuilder.CreateTable(
                name: "Push",
                columns: table => new
                {
                    PushId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AiScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ScheduleEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Push", x => x.PushId);
                    table.ForeignKey(
                        name: "FK_Push_AiSchedules_AiScheduleId",
                        column: x => x.AiScheduleId,
                        principalTable: "AiSchedules",
                        principalColumn: "AiScheduleId");
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemeAudios",
                columns: table => new
                {
                    AlarmSchemeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AudioId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchemeAudios", x => new { x.AlarmSchemeId, x.AudioId });
                    table.ForeignKey(
                        name: "FK_AlarmSchemeAudios_AlarmSchemes_AlarmSchemeId",
                        column: x => x.AlarmSchemeId,
                        principalTable: "AlarmSchemes",
                        principalColumn: "AlarmSchemeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlarmSchemeAudios_Audios_AudioId",
                        column: x => x.AudioId,
                        principalTable: "Audios",
                        principalColumn: "AudioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemeBuzzers",
                columns: table => new
                {
                    AlarmSchemeId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuzzerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchemeBuzzers", x => new { x.AlarmSchemeId, x.BuzzerId });
                    table.ForeignKey(
                        name: "FK_AlarmSchemeBuzzers_AlarmSchemes_AlarmSchemeId",
                        column: x => x.AlarmSchemeId,
                        principalTable: "AlarmSchemes",
                        principalColumn: "AlarmSchemeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlarmSchemeBuzzers_Buzzers_BuzzerId",
                        column: x => x.BuzzerId,
                        principalTable: "Buzzers",
                        principalColumn: "BuzzerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlarmSchemePushes",
                columns: table => new
                {
                    AlarmSchemeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PushId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmSchemePushes", x => new { x.AlarmSchemeId, x.PushId });
                    table.ForeignKey(
                        name: "FK_AlarmSchemePushes_AlarmSchemes_AlarmSchemeId",
                        column: x => x.AlarmSchemeId,
                        principalTable: "AlarmSchemes",
                        principalColumn: "AlarmSchemeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlarmSchemePushes_Push_PushId",
                        column: x => x.PushId,
                        principalTable: "Push",
                        principalColumn: "PushId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemeAudios_AudioId",
                table: "AlarmSchemeAudios",
                column: "AudioId");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemeBuzzers_BuzzerId",
                table: "AlarmSchemeBuzzers",
                column: "BuzzerId");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemePushes_PushId",
                table: "AlarmSchemePushes",
                column: "PushId");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmSchemes_AlarmSchemeTypeId",
                table: "AlarmSchemes",
                column: "AlarmSchemeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Audios_AiScheduleId",
                table: "Audios",
                column: "AiScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Buzzers_AiScheduleId",
                table: "Buzzers",
                column: "AiScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Push_AiScheduleId",
                table: "Push",
                column: "AiScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlarmSchemeAudios");

            migrationBuilder.DropTable(
                name: "AlarmSchemeBuzzers");

            migrationBuilder.DropTable(
                name: "AlarmSchemePushes");

            migrationBuilder.DropTable(
                name: "AlarmSchemeTypes");

            migrationBuilder.DropTable(
                name: "AlarmUsers");

            migrationBuilder.DropTable(
                name: "Audios");

            migrationBuilder.DropTable(
                name: "Buzzers");

            migrationBuilder.DropTable(
                name: "AlarmSchemes");

            migrationBuilder.DropTable(
                name: "Push");

            migrationBuilder.DropTable(
                name: "AiSchedules");
        }
    }
}
