using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationDateToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Animals",
                type: "datetime",
                nullable: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Animals"
            );
        }
    }
}
