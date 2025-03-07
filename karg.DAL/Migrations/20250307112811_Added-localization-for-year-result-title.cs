using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addedlocalizationforyearresulttitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "YearsResults");

            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "YearsResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_YearsResults_TitleId",
                table: "YearsResults",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_YearsResults_LocalizationsSets_TitleId",
                table: "YearsResults",
                column: "TitleId",
                principalTable: "LocalizationsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YearsResults_LocalizationsSets_TitleId",
                table: "YearsResults");

            migrationBuilder.DropIndex(
                name: "IX_YearsResults_TitleId",
                table: "YearsResults");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "YearsResults");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "YearsResults",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
