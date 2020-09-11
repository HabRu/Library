using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class AddInitializeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authtor", "Description", "Genre", "Image", "Language", "Publisher", "Status", "Title", "Year" },
                values: new object[] { 1, "Борис Акунин", "Детективный роман Бориса Акунина, действие которого разворачивается на фоне грозных событий войны 1812 года, является художественным приложением к седьмому тому проекта «История Российского государства». Такой пары сыщиков в истории криминального жанра, кажется, еще не было", "Детектив", "/images/миривойна.jpg", "Русский", "Литрес", 0, "Мир и война", 0 });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Average", "BookId", "Users" },
                values: new object[] { 1, (byte)0, 1, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authtor", "Description", "Genre", "Image", "Language", "Publisher", "Status", "Title", "Year" },
                values: new object[] { 3, "Борис Акунин", "Детективный роман Бориса Акунина, действие которого разворачивается на фоне грозных событий войны 1812 года, является художественным приложением к седьмому тому проекта «История Российского государства». Такой пары сыщиков в истории криминального жанра, кажется, еще не было", "Детектив", "image/миривойна", "Русский", null, 0, "Мир и война", 0 });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Average", "BookId", "Users" },
                values: new object[] { 3, (byte)0, 3, null });
        }
    }
}
