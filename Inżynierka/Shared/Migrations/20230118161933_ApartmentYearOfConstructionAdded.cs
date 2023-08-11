using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class ApartmentYearOfConstructionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfConstruction",
                table: "ApartmentSaleOffer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfConstruction",
                table: "ApartmentRentOffer",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourite_Offer_OfferId",
                table: "UserFavourite",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourite_Offer_OfferId",
                table: "UserFavourite");

            migrationBuilder.DropColumn(
                name: "YearOfConstruction",
                table: "ApartmentSaleOffer");

            migrationBuilder.DropColumn(
                name: "YearOfConstruction",
                table: "ApartmentRentOffer");
        }
    }
}
