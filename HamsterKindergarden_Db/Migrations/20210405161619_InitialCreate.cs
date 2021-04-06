using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HamsterKindergarden_Db.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activitieCages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tickcounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activitieCages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hamstercage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hamstercage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "hamster",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LatestActivities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AktivitesCounter = table.Column<int>(type: "int", nullable: false),
                    HamsterCageId = table.Column<int>(type: "int", nullable: true),
                    ActivitieCageid = table.Column<int>(type: "int", nullable: true),
                    Countminutes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hamster", x => x.ID);
                    table.ForeignKey(
                        name: "FK_hamster_activitieCages_ActivitieCageid",
                        column: x => x.ActivitieCageid,
                        principalTable: "activitieCages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_hamster_hamstercage_HamsterCageId",
                        column: x => x.HamsterCageId,
                        principalTable: "hamstercage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AktivityLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HamsterActivity = table.Column<int>(type: "int", nullable: false),
                    ActivityTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    HamsterID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AktivityLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AktivityLog_hamster_HamsterID",
                        column: x => x.HamsterID,
                        principalTable: "hamster",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AktivityLog_HamsterID",
                table: "AktivityLog",
                column: "HamsterID");

            migrationBuilder.CreateIndex(
                name: "IX_hamster_ActivitieCageid",
                table: "hamster",
                column: "ActivitieCageid");

            migrationBuilder.CreateIndex(
                name: "IX_hamster_HamsterCageId",
                table: "hamster",
                column: "HamsterCageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AktivityLog");

            migrationBuilder.DropTable(
                name: "hamster");

            migrationBuilder.DropTable(
                name: "activitieCages");

            migrationBuilder.DropTable(
                name: "hamstercage");
        }
    }
}
