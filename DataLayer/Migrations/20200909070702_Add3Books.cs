using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class Add3Books : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authtor", "Description", "Genre", "Image", "Language", "Publisher", "Status", "Title", "Year" },
                values: new object[] { 3, "Майк Омер", "На мосту в Чикаго, облокотившись на перила, стоит молодая красивая женщина. Очень бледная и очень грустная. Она неподвижно смотрит на темную воду, прикрывая ладонью плачущие глаза. И никому не приходит в голову, что", "Детектив", "/images/внутриубийцы.jpg", "Русский", "Литрес", 0, "Внутри убийцы", 0 });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Average", "BookId", "Users" },
                values: new object[] { 3, (byte)0, 3, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
