using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.infrastructure.Migrations.TenantCrmDb
{
    /// <inheritdoc />
    public partial class AddTermsCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TermsAndConditions_Version",
                table: "TermsAndConditions");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByRoleCode",
                table: "TermsAndConditions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "TermsAndConditions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndConditions_CreatedByRoleCode_Version",
                table: "TermsAndConditions",
                columns: new[] { "CreatedByRoleCode", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TermsAndConditions_CreatedByRoleCode_Version",
                table: "TermsAndConditions");

            migrationBuilder.DropColumn(
                name: "CreatedByRoleCode",
                table: "TermsAndConditions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "TermsAndConditions");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndConditions_Version",
                table: "TermsAndConditions",
                column: "Version",
                unique: true);
        }
    }
}
