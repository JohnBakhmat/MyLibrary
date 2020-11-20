using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLibrary.Data.Migrations
{
    public partial class bookNumberFiix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookNumber",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookNumber",
                table: "Books");
        }
    }
}
