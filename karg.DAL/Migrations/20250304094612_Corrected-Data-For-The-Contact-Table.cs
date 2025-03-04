using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedDataForTheContactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Category",
                value: "LocationUa");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Value" },
                values: new object[] { "LocationUa", "Kyiv" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Instagram", "https://www.instagram.com/karg.kyiv?fbclid=IwAR1OSBKSNd-YuMMDs0Wk4yX4wnH9YZFfNU9RRpG5fhI1uQQh-cmGZV29hlg" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Facebook", "https://www.facebook.com/KARG.kyivanimalrescuegroup/?locale=ua_UA" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Telegram", "Посилання на телеграм" });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Category", "Value" },
                values: new object[,]
                {
                    { 9, "Statistics", "2427" },
                    { 10, "Statistics", "2300" },
                    { 11, "Statistics", "720" },
                    { 12, "Statistics", "115" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Category",
                value: "Location");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Instagram", "https://www.instagram.com/karg.kyiv?fbclid=IwAR1OSBKSNd-YuMMDs0Wk4yX4wnH9YZFfNU9RRpG5fhI1uQQh-cmGZV29hlg" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Facebook", "https://www.facebook.com/KARG.kyivanimalrescuegroup/?locale=ua_UA" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Telegram", "Посилання на телеграм" });

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Value" },
                values: new object[] { "Statistics", "2427 2300 720 115" });
        }
    }
}
