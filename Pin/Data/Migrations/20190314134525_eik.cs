using Microsoft.EntityFrameworkCore.Migrations;

namespace Pin.Data.Migrations
{
    public partial class eik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EIK",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EIK",
                table: "Companies");
        }
    }
}
