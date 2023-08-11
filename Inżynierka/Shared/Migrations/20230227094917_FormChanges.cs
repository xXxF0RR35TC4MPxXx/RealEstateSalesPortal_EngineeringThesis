using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class FormChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientComment",
                table: "SingleFormProposal");

            migrationBuilder.DropColumn(
                name: "Rejected",
                table: "SingleFormProposal");

            migrationBuilder.AddColumn<string>(
                name: "ClientComment",
                table: "UserPreferenceForm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmailVerificationGuid",
                table: "UserPreferenceForm",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientComment",
                table: "UserPreferenceForm");

            migrationBuilder.DropColumn(
                name: "EmailVerificationGuid",
                table: "UserPreferenceForm");

            migrationBuilder.AddColumn<string>(
                name: "ClientComment",
                table: "SingleFormProposal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Rejected",
                table: "SingleFormProposal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
