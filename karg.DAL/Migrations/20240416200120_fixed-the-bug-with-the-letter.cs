using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    public partial class fixedthebugwiththeletter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Сategory",
                table: "Animal",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Animal",
                newName: "Сategory");
        }
    }
}
