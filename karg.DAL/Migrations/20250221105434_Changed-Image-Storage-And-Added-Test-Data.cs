using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImageStorageAndAddedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "Token" },
                values: new object[] { 1, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGdWxsbmFtZSI6IkFkbWluIEtBUkciLCJSb2xlIjoiRGlyZWN0b3IiLCJFbWFpbCI6ImFkbWluQGdtYWlsLmNvbSIsImV4cCI6MzMyNTYxODA4OTEsImlzcyI6ImthcmcuY29tIiwiYXVkIjoia2FyZy5jb20ifQ.2RcVCXa9B_xS3zBEBTEAFsEyfS0DIpyWQtIBxs3IabM" });

            migrationBuilder.InsertData(
                table: "Rescuers",
                columns: new[] { "Id", "Current_Password", "Email", "FullName", "PhoneNumber", "Previous_Password", "Role", "TokenId" },
                values: new object[] { 1, "001He87I8P1n8k7a70SJizxEyQdPQsTGcSOgRls0V8Y=", "admin@gmail.com", "Admin KARG", null, null, "Director", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rescuers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
