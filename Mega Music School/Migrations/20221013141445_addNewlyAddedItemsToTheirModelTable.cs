using Microsoft.EntityFrameworkCore.Migrations;

namespace Mega_Music_School.Migrations
{
    public partial class addNewlyAddedItemsToTheirModelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mp3Upload",
                table: "Videos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mp3Upload",
                table: "Videos");
        }
    }
}
