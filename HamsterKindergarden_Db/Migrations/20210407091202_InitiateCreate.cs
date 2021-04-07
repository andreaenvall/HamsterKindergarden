using Microsoft.EntityFrameworkCore.Migrations;

namespace HamsterKindergarden_Db.Migrations
{
    public partial class InitiateCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HamsterCount",
                table: "hamstercage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HamsterCount",
                table: "hamstercage");
        }
    }
}
