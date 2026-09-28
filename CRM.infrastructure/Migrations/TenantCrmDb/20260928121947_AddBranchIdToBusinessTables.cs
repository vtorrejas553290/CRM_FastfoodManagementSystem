using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.infrastructure.Migrations.TenantCrmDb
{
    /// <inheritdoc />
    public partial class AddBranchIdToBusinessTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "CustomerFeedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ActivityLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BranchId",
                table: "Transactions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BranchId",
                table: "Customers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedbacks_BranchId",
                table: "CustomerFeedbacks",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_BranchId",
                table: "Complaints",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_BranchId",
                table: "ActivityLogs",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLogs_Branches_BranchId",
                table: "ActivityLogs",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Branches_BranchId",
                table: "Complaints",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedbacks_Branches_BranchId",
                table: "CustomerFeedbacks",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Branches_BranchId",
                table: "Transactions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLogs_Branches_BranchId",
                table: "ActivityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Branches_BranchId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedbacks_Branches_BranchId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Branches_BranchId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BranchId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BranchId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BranchId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedbacks_BranchId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_BranchId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLogs_BranchId",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CustomerFeedbacks");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ActivityLogs");
        }
    }
}
