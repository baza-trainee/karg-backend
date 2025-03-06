using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Changedtheyearresulttablefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                    name: "Created_At",
                    table: "YearsResults",
                    nullable: false,
                    defaultValueSql: null);

            migrationBuilder.DropColumn(
                name: "Year",
                table: "YearsResults");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "YearsResults",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "YearsResults");

            migrationBuilder.AddColumn<int>(
                   name: "Year",
                   table: "YearsResults",
                   nullable: false,
                   defaultValue: null);

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "YearsResults");
        }
    }
}
