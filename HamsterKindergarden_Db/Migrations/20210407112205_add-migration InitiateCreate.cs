using Microsoft.EntityFrameworkCore.Migrations;

namespace HamsterKindergarden_Db.Migrations
{
    public partial class addmigrationInitiateCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMale",
                table: "hamstercage",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMale",
                table: "hamstercage");
        }
    }
}
