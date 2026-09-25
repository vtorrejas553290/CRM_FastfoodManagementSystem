using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.infrastructure.Migrations.TenantCrmDb
{
    /// <inheritdoc />
    public partial class AddUserTermsAcceptance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTermsAcceptances",
                columns: table => new
                {
                    UserTermsAcceptanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermsAndConditionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTermsAcceptances", x => x.UserTermsAcceptanceId);
                    table.ForeignKey(
                        name: "FK_UserTermsAcceptances_TermsAndConditions_TermsAndConditionId",
                        column: x => x.TermsAndConditionId,
                        principalTable: "TermsAndConditions",
                        principalColumn: "TermsAndConditionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTermsAcceptances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTermsAcceptances_TermsAndConditionId",
                table: "UserTermsAcceptances",
                column: "TermsAndConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTermsAcceptances_UserId_TermsAndConditionId",
                table: "UserTermsAcceptances",
                columns: new[] { "UserId", "TermsAndConditionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTermsAcceptances");
        }
    }
}
