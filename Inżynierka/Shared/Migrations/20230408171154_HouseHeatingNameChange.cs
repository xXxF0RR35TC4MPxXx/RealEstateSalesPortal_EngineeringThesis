using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class HouseHeatingNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtherHeating",
                table: "HouseSaleOffer",
                newName: "FireplaceHeating");

            migrationBuilder.RenameColumn(
                name: "OtherHeating",
                table: "HouseRentOffer",
                newName: "FireplaceHeating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FireplaceHeating",
                table: "HouseSaleOffer",
                newName: "OtherHeating");

            migrationBuilder.RenameColumn(
                name: "FireplaceHeating",
                table: "HouseRentOffer",
                newName: "OtherHeating");
        }
    }
}
