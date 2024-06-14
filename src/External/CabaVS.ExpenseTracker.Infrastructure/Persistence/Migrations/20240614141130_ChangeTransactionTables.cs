using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTransactionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTransaction_Balances_SourceId",
                table: "ExpenseTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTransaction_ExpenseCategories_DestinationId",
                table: "ExpenseTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeTransaction_Balances_DestinationId",
                table: "IncomeTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeTransaction_IncomeCategories_SourceId",
                table: "IncomeTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferTransaction_Balances_DestinationId",
                table: "TransferTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferTransaction_Balances_SourceId",
                table: "TransferTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransferTransaction",
                table: "TransferTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomeTransaction",
                table: "IncomeTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseTransaction",
                table: "ExpenseTransaction");

            migrationBuilder.RenameTable(
                name: "TransferTransaction",
                newName: "TransferTransactions");

            migrationBuilder.RenameTable(
                name: "IncomeTransaction",
                newName: "IncomeTransactions");

            migrationBuilder.RenameTable(
                name: "ExpenseTransaction",
                newName: "ExpenseTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_TransferTransaction_SourceId",
                table: "TransferTransactions",
                newName: "IX_TransferTransactions_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferTransaction_DestinationId",
                table: "TransferTransactions",
                newName: "IX_TransferTransactions_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomeTransaction_SourceId",
                table: "IncomeTransactions",
                newName: "IX_IncomeTransactions_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomeTransaction_DestinationId",
                table: "IncomeTransactions",
                newName: "IX_IncomeTransactions_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseTransaction_SourceId",
                table: "ExpenseTransactions",
                newName: "IX_ExpenseTransactions_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseTransaction_DestinationId",
                table: "ExpenseTransactions",
                newName: "IX_ExpenseTransactions_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransferTransactions",
                table: "TransferTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomeTransactions",
                table: "IncomeTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseTransactions",
                table: "ExpenseTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTransactions_Balances_SourceId",
                table: "ExpenseTransactions",
                column: "SourceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTransactions_ExpenseCategories_DestinationId",
                table: "ExpenseTransactions",
                column: "DestinationId",
                principalTable: "ExpenseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeTransactions_Balances_DestinationId",
                table: "IncomeTransactions",
                column: "DestinationId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeTransactions_IncomeCategories_SourceId",
                table: "IncomeTransactions",
                column: "SourceId",
                principalTable: "IncomeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferTransactions_Balances_DestinationId",
                table: "TransferTransactions",
                column: "DestinationId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferTransactions_Balances_SourceId",
                table: "TransferTransactions",
                column: "SourceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTransactions_Balances_SourceId",
                table: "ExpenseTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTransactions_ExpenseCategories_DestinationId",
                table: "ExpenseTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeTransactions_Balances_DestinationId",
                table: "IncomeTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeTransactions_IncomeCategories_SourceId",
                table: "IncomeTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferTransactions_Balances_DestinationId",
                table: "TransferTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferTransactions_Balances_SourceId",
                table: "TransferTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransferTransactions",
                table: "TransferTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomeTransactions",
                table: "IncomeTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseTransactions",
                table: "ExpenseTransactions");

            migrationBuilder.RenameTable(
                name: "TransferTransactions",
                newName: "TransferTransaction");

            migrationBuilder.RenameTable(
                name: "IncomeTransactions",
                newName: "IncomeTransaction");

            migrationBuilder.RenameTable(
                name: "ExpenseTransactions",
                newName: "ExpenseTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_TransferTransactions_SourceId",
                table: "TransferTransaction",
                newName: "IX_TransferTransaction_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferTransactions_DestinationId",
                table: "TransferTransaction",
                newName: "IX_TransferTransaction_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomeTransactions_SourceId",
                table: "IncomeTransaction",
                newName: "IX_IncomeTransaction_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomeTransactions_DestinationId",
                table: "IncomeTransaction",
                newName: "IX_IncomeTransaction_DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseTransactions_SourceId",
                table: "ExpenseTransaction",
                newName: "IX_ExpenseTransaction_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseTransactions_DestinationId",
                table: "ExpenseTransaction",
                newName: "IX_ExpenseTransaction_DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransferTransaction",
                table: "TransferTransaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomeTransaction",
                table: "IncomeTransaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseTransaction",
                table: "ExpenseTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTransaction_Balances_SourceId",
                table: "ExpenseTransaction",
                column: "SourceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTransaction_ExpenseCategories_DestinationId",
                table: "ExpenseTransaction",
                column: "DestinationId",
                principalTable: "ExpenseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeTransaction_Balances_DestinationId",
                table: "IncomeTransaction",
                column: "DestinationId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeTransaction_IncomeCategories_SourceId",
                table: "IncomeTransaction",
                column: "SourceId",
                principalTable: "IncomeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferTransaction_Balances_DestinationId",
                table: "TransferTransaction",
                column: "DestinationId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferTransaction_Balances_SourceId",
                table: "TransferTransaction",
                column: "SourceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
