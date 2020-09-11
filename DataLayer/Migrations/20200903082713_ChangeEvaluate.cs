using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class ChangeEvaluate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Authtor",
                table: "Books",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authtor", "Description", "Genre", "Image", "Language", "Publisher", "Status", "Title", "Year" },
                values: new object[] { 3, "Борис Акунин", "Детективный роман Бориса Акунина, действие которого разворачивается на фоне грозных событий войны 1812 года, является художественным приложением к седьмому тому проекта «История Российского государства». Такой пары сыщиков в истории криминального жанра, кажется, еще не было", "Детектив", "image/миривойна", "Русский", null, 0, "Мир и война", 0 });

            migrationBuilder.InsertData(
                table: "Evaluations",
                columns: new[] { "Id", "Average", "BookId", "Users" },
                values: new object[] { 3, (byte)0, 3, null });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookIdentificator",
                table: "Reservations",
                column: "BookIdentificator",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Books_BookIdentificator",
                table: "Reservations",
                column: "BookIdentificator",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Books_BookIdentificator",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookIdentificator",
                table: "Reservations");

            migrationBuilder.DeleteData(
                table: "Evaluations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Authtor",
                table: "Books",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
