using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.infrastructure.Migrations.TenantCrmDb
{
    /// <inheritdoc />
    public partial class AddRetentionOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RetentionOffers",
                columns: table => new
                {
                    RetentionOfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PromotionId = table.Column<int>(type: "int", nullable: true),
                    OfferText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetentionOffers", x => x.RetentionOfferId);
                    table.ForeignKey(
                        name: "FK_RetentionOffers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RetentionOffers_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "PromotionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RetentionOffers_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RetentionOffers_CreatedByUserId",
                table: "RetentionOffers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RetentionOffers_CustomerId",
                table: "RetentionOffers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RetentionOffers_PromotionId",
                table: "RetentionOffers",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_RetentionOffers_Status",
                table: "RetentionOffers",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetentionOffers");
        }
    }
}
