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
                    ActivitieCageid = table.Column<int>(type: "int", nullable: true),
                    HamsterCageId = table.Column<int>(type: "int", nullable: true)
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
                name: "hamster");

            migrationBuilder.DropTable(
                name: "activitieCages");

            migrationBuilder.DropTable(
                name: "hamstercage");
        }
    }
}
