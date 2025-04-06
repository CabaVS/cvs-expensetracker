using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabaVS.ExpenseTracker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountInSourceCurrency = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountInDestinationCurrency = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DestinationBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DestinationCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Balances_DestinationBalanceId",
                        column: x => x.DestinationBalanceId,
                        principalTable: "Balances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Balances_SourceBalanceId",
                        column: x => x.SourceBalanceId,
                        principalTable: "Balances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_DestinationCategoryId",
                        column: x => x.DestinationCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_SourceCategoryId",
                        column: x => x.SourceCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DestinationBalanceId",
                table: "Transactions",
                column: "DestinationBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DestinationCategoryId",
                table: "Transactions",
                column: "DestinationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceBalanceId",
                table: "Transactions",
                column: "SourceBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceCategoryId",
                table: "Transactions",
                column: "SourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WorkspaceId",
                table: "Transactions",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
