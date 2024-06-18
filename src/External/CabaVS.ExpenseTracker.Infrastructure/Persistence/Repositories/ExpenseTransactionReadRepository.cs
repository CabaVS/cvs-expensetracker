using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseTransactionReadRepository(
    SqlConnectionFactory sqlConnectionFactory) : IExpenseTransactionReadRepository
{
    public async Task<ExpenseTransactionModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT
                               [et].[Id], [et].[Date], [et].[AmountInSourceCurrency], [et].[AmountInDestinationCurrency], [et].[Tags],
                               [ec].[Id], [ec].[Name],
                                   [ecc].[Id], [ecc].[Name], [ecc].[Code], [ecc].[Symbol],
                               [b].[Id], [b].[Name], [b].[Amount],
                                   [bc].[Id], [bc].[Name], [bc].[Code], [bc].[Symbol]
                           FROM [dbo].[ExpenseTransactions] AS [et]
                           CROSS APPLY STRING_SPLIT([et].[Tags], ',') AS t
                           INNER JOIN [dbo].[ExpenseCategories] AS [ec] ON [ec].[Id] = [et].[DestinationId] AND [ec].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [ecc] ON [ecc].[Id] = [ec].[CurrencyId]
                           INNER JOIN [dbo].[Balances] AS [b] ON [b].[Id] = [et].[SourceId] AND [b].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [bc] ON [bc].[Id] = [b].[CurrencyId]
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync(
            sql,
            MapExpenseCategoryModel,
            new { workspaceId },
            splitOn: "Id,Id,Id,Id");
        return models.Select(x => MapToApplicationModel(x)!).ToArray();
    }

    public async Task<ExpenseTransactionModel?> GetById(Guid expenseTransactionId, Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1)
                               [et].[Id], [et].[Date], [et].[AmountInSourceCurrency], [et].[AmountInDestinationCurrency], [et].[Tags],
                               [ec].[Id], [ec].[Name],
                                   [ecc].[Id], [ecc].[Name], [ecc].[Code], [ecc].[Symbol],
                               [b].[Id], [b].[Name], [b].[Amount],
                                   [bc].[Id], [bc].[Name], [bc].[Code], [bc].[Symbol]
                           FROM [dbo].[ExpenseTransactions] AS [et]
                           INNER JOIN [dbo].[ExpenseCategories] AS [ec] ON [ec].[Id] = [et].[DestinationId] AND [ec].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [ecc] ON [ecc].[Id] = [ec].[CurrencyId]
                           INNER JOIN [dbo].[Balances] AS [b] ON [b].[Id] = [et].[SourceId] AND [b].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [bc] ON [bc].[Id] = [b].[CurrencyId]
                           WHERE [et].[Id] = @expenseTransactionId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync(
            sql,
            MapExpenseCategoryModel,
            new { expenseTransactionId, workspaceId },
            splitOn: "Id,Id,Id,Id");
        return MapToApplicationModel(models.SingleOrDefault());
    }
    
    private static readonly Func<ExpenseTransactionTableEntry, ExpenseCategoryModel, CurrencyModel, BalanceModel, CurrencyModel, ExpenseTransactionTableEntry> MapExpenseCategoryModel =
        (et, ec, ecc, b, bc) => et with
        {
            Destination = ec with { Currency = ecc },
            Source = b with { Currency = bc }
        };

    private static ExpenseTransactionModel? MapToApplicationModel(ExpenseTransactionTableEntry? tableEntry) =>
        tableEntry is not null
            ? new ExpenseTransactionModel(
                tableEntry.Id,
                tableEntry.Date,
                tableEntry.Source,
                tableEntry.AmountInSourceCurrency,
                tableEntry.Destination,
                tableEntry.AmountInDestinationCurrency,
                tableEntry.Tags.Split(','))
            : null;
    
    private sealed record ExpenseTransactionTableEntry(
        Guid Id,
        DateOnly Date,
        decimal AmountInSourceCurrency,
        decimal AmountInDestinationCurrency,
        string Tags)
    {
        public required BalanceModel Source { get; init; }
        public required ExpenseCategoryModel Destination { get; init; }
    }
}