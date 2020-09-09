using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class AddedNewBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authtor", "Description", "Genre", "Image", "Language", "Publisher", "Status", "Title", "Year" },
                values: new object[] { 2, "Александра Маринина", "Программа против Cистемы. Системы всесильной и насквозь коррумпированной, на все имеющей цену и при этом ничего неспособной ценить по-настоящему. Возможно ли такое?", "Детектив", "/images/ценавопроса.jpg", "Русский", "Литрес", 0, "Цена вопросв", 0 });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Average", "BookId", "Users" },
                values: new object[] { 2, (byte)0, 2, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
