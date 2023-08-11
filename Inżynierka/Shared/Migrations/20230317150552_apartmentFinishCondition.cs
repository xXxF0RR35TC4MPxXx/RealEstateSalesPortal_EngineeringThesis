using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class apartmentFinishCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApartmentFinishCondition",
                table: "ApartmentSaleOffer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentFinishCondition",
                table: "ApartmentRentOffer",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApartmentFinishCondition",
                table: "ApartmentSaleOffer");

            migrationBuilder.DropColumn(
                name: "ApartmentFinishCondition",
                table: "ApartmentRentOffer");
        }
    }
}
