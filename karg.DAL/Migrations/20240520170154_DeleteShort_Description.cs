using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    public partial class DeleteShort_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animal_LocalizationSet_Short_DescriptionId",
                table: "Animal");

            migrationBuilder.DropIndex(
                name: "IX_Animal_Short_DescriptionId",
                table: "Animal");

            migrationBuilder.DropColumn(
                name: "Short_DescriptionId",
                table: "Animal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Short_DescriptionId",
                table: "Animal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Animal_Short_DescriptionId",
                table: "Animal",
                column: "Short_DescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animal_LocalizationSet_Short_DescriptionId",
                table: "Animal",
                column: "Short_DescriptionId",
                principalTable: "LocalizationSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
