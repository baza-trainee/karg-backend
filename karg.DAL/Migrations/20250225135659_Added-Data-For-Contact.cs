using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataForContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Category", "Value" },
                values: new object[,]
                {
                    { 1, "PhoneNumber", "+38 (093) 986-2262" },
                    { 2, "PhoneNumber", "+38 (098) 844-7937" },
                    { 3, "Email", "karg.inform@gmail.com" },
                    { 4, "Location", "м. Київ" },
                    { 5, "Instagram", "https://www.instagram.com/karg.kyiv?fbclid=IwAR1OSBKSNd-YuMMDs0Wk4yX4wnH9YZFfNU9RRpG5fhI1uQQh-cmGZV29hlg" },
                    { 6, "Facebook", "https://www.facebook.com/KARG.kyivanimalrescuegroup/?locale=ua_UA" },
                    { 7, "Telegram", "Посилання на телеграм" },
                    { 8, "Statistics", "2427 2300 720 115" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
