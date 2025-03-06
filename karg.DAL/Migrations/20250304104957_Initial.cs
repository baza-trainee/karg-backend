using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace karg.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LocalizationsSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationsSets", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Uri = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Advices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advices_LocalizationsSets_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advices_LocalizationsSets_TitleId",
                        column: x => x.TitleId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NameId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    StoryId = table.Column<int>(type: "int", nullable: false),
                    Donats = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_LocalizationsSets_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animals_LocalizationsSets_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animals_LocalizationsSets_StoryId",
                        column: x => x.StoryId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQs_LocalizationsSets_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FAQs_LocalizationsSets_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    LocalizationSetId = table.Column<int>(type: "int", nullable: false),
                    CultureCode = table.Column<string>(type: "varchar(2)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => new { x.LocalizationSetId, x.CultureCode });
                    table.ForeignKey(
                        name: "FK_Localizations_Cultures_CultureCode",
                        column: x => x.CultureCode,
                        principalTable: "Cultures",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Localizations_LocalizationsSets_LocalizationSetId",
                        column: x => x.LocalizationSetId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "YearsResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<DateOnly>(type: "date", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearsResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearsResults_LocalizationsSets_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "LocalizationsSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rescuers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Current_Password = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Previous_Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rescuers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rescuers_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Uri = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdviceId = table.Column<int>(type: "int", nullable: true),
                    AnimalId = table.Column<int>(type: "int", nullable: true),
                    RescuerId = table.Column<int>(type: "int", nullable: true),
                    PartnerId = table.Column<int>(type: "int", nullable: true),
                    YearResultId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Advices_AdviceId",
                        column: x => x.AdviceId,
                        principalTable: "Advices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Rescuers_RescuerId",
                        column: x => x.RescuerId,
                        principalTable: "Rescuers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_YearsResults_YearResultId",
                        column: x => x.YearResultId,
                        principalTable: "YearsResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Category", "Value" },
                values: new object[,]
                {
                    { 1, "PhoneNumber", "+38 (093) 986-2262" },
                    { 2, "PhoneNumber", "+38 (098) 844-7937" },
                    { 3, "Email", "karg.inform@gmail.com" },
                    { 4, "LocationUa", "м. Київ" },
                    { 5, "LocationEn", "Kyiv" },
                    { 6, "Instagram", "https://www.instagram.com/karg.kyiv?fbclid=IwAR1OSBKSNd-YuMMDs0Wk4yX4wnH9YZFfNU9RRpG5fhI1uQQh-cmGZV29hlg" },
                    { 7, "Facebook", "https://www.facebook.com/KARG.kyivanimalrescuegroup/?locale=ua_UA" },
                    { 8, "Telegram", "Посилання на телеграм" },
                    { 9, "Statistics", "2427" },
                    { 10, "Statistics", "2300" },
                    { 11, "Statistics", "720" },
                    { 12, "Statistics", "115" }
                });

            migrationBuilder.InsertData(
                table: "Cultures",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "en", "English" },
                    { "ua", "Ukrainian" }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "Token" },
                values: new object[] { 1, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGdWxsbmFtZSI6IkFkbWluIEtBUkciLCJSb2xlIjoiRGlyZWN0b3IiLCJFbWFpbCI6ImFkbWluQGdtYWlsLmNvbSIsImV4cCI6MzMyNTYxODA4OTEsImlzcyI6ImthcmcuY29tIiwiYXVkIjoia2FyZy5jb20ifQ.2RcVCXa9B_xS3zBEBTEAFsEyfS0DIpyWQtIBxs3IabM" });

            migrationBuilder.InsertData(
                table: "Rescuers",
                columns: new[] { "Id", "Current_Password", "Email", "FullName", "PhoneNumber", "Previous_Password", "Role", "TokenId" },
                values: new object[] { 1, "001He87I8P1n8k7a70SJizxEyQdPQsTGcSOgRls0V8Y=", "admin@gmail.com", "Admin KARG", null, null, "Director", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Advices_DescriptionId",
                table: "Advices",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Advices_TitleId",
                table: "Advices",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_DescriptionId",
                table: "Animals",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_NameId",
                table: "Animals",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_StoryId",
                table: "Animals",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_AnswerId",
                table: "FAQs",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_QuestionId",
                table: "FAQs",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AdviceId",
                table: "Images",
                column: "AdviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AnimalId",
                table: "Images",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PartnerId",
                table: "Images",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_RescuerId",
                table: "Images",
                column: "RescuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_YearResultId",
                table: "Images",
                column: "YearResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Localizations_CultureCode",
                table: "Localizations",
                column: "CultureCode");

            migrationBuilder.CreateIndex(
                name: "IX_Rescuers_TokenId",
                table: "Rescuers",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YearsResults_DescriptionId",
                table: "YearsResults",
                column: "DescriptionId");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropTable(
                name: "Advices");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Rescuers");

            migrationBuilder.DropTable(
                name: "YearsResults");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "LocalizationsSets");
            */
        }
    }
}
