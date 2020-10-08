using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class ChangeReserv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookIdentificator",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookIdentificator",
                table: "Reservations",
                column: "BookIdentificator");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookIdentificator",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookIdentificator",
                table: "Reservations",
                column: "BookIdentificator",
                unique: true);
        }
    }
}
