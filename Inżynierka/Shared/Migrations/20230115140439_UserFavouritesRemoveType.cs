using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UserFavouritesRemoveType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavourite",
                table: "UserFavourite");

            migrationBuilder.DropColumn(
                name: "EstateAndOfferType",
                table: "UserFavourite");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavourite",
                table: "UserFavourite",
                columns: new[] { "OfferId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavourite",
                table: "UserFavourite");

            migrationBuilder.AddColumn<string>(
                name: "EstateAndOfferType",
                table: "UserFavourite",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavourite",
                table: "UserFavourite",
                columns: new[] { "OfferId", "EstateAndOfferType", "UserId" });
        }
    }
}
