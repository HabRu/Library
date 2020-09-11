using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class Add4Books : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authtor", "Description", "Genre", "Image", "Language", "Publisher", "Status", "Title", "Year" },
                values: new object[] { 4, "Иван Петров", "Эывыфвыфвыфвыфваыавы", "Комедия", "/images/NoImage.jpg", "Русский", "Старт", 0, "Мир и дружба", 0 });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Average", "BookId", "Users" },
                values: new object[] { 4, (byte)0, 4, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
