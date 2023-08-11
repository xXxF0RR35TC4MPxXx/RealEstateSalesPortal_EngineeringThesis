using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UserPreferenceForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPreferenceForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinArea = table.Column<int>(type: "int", nullable: true),
                    MaxArea = table.Column<int>(type: "int", nullable: true),
                    RoomCount = table.Column<int>(type: "int", nullable: true),
                    MaxPrice = table.Column<int>(type: "int", nullable: true),
                    ClientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferenceForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferenceForm_User_AgentId",
                        column: x => x.AgentId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SingleFormProposal",
                columns: table => new
                {
                    FormId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    Rejected = table.Column<bool>(type: "bit", nullable: false),
                    ClientComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleFormProposal", x => new { x.OfferId, x.FormId });
                    table.ForeignKey(
                        name: "FK_SingleFormProposal_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SingleFormProposal_UserPreferenceForm_FormId",
                        column: x => x.FormId,
                        principalTable: "UserPreferenceForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleFormProposal_FormId",
                table: "SingleFormProposal",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceForm_AgentId",
                table: "UserPreferenceForm",
                column: "AgentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleFormProposal");

            migrationBuilder.DropTable(
                name: "UserPreferenceForm");
        }
    }
}
