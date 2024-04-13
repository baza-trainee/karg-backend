using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    public partial class Addedphototorescuer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Rescuer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rescuer_ImageId",
                table: "Rescuer",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rescuer_Image_ImageId",
                table: "Rescuer",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rescuer_Image_ImageId",
                table: "Rescuer");

            migrationBuilder.DropIndex(
                name: "IX_Rescuer_ImageId",
                table: "Rescuer");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Rescuer");
        }
    }
}
