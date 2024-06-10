using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInSourceCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInDestinationCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseTransaction_Balances_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Balances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseTransaction_ExpenseCategories_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInSourceCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInDestinationCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeTransaction_Balances_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Balances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeTransaction_IncomeCategories_SourceId",
                        column: x => x.SourceId,
                        principalTable: "IncomeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInSourceCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInDestinationCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferTransaction_Balances_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Balances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferTransaction_Balances_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Balances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransaction_DestinationId",
                table: "ExpenseTransaction",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransaction_SourceId",
                table: "ExpenseTransaction",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTransaction_DestinationId",
                table: "IncomeTransaction",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTransaction_SourceId",
                table: "IncomeTransaction",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferTransaction_DestinationId",
                table: "TransferTransaction",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferTransaction_SourceId",
                table: "TransferTransaction",
                column: "SourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseTransaction");

            migrationBuilder.DropTable(
                name: "IncomeTransaction");

            migrationBuilder.DropTable(
                name: "TransferTransaction");
        }
    }
}
