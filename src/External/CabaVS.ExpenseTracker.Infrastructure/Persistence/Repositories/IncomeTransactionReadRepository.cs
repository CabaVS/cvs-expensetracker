using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeTransactionReadRepository(
    SqlConnectionFactory sqlConnectionFactory) : IIncomeTransactionReadRepository
{
    public async Task<IncomeTransactionModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT
                               [it].[Id], [it].[Date], [it].[AmountInSourceCurrency], [it].[AmountInDestinationCurrency], [it].[Tags],
                               [ic].[Id], [ic].[Name],
                                   [icc].[Id], [icc].[Name], [icc].[Code], [icc].[Symbol],
                               [b].[Id], [b].[Name], [b].[Amount],
                                   [bc].[Id], [bc].[Name], [bc].[Code], [bc].[Symbol]
                           FROM [dbo].[IncomeTransactions] AS [it]
                           INNER JOIN [dbo].[IncomeCategories] AS [ic] ON [ic].[Id] = [it].[SourceId] AND [ic].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [icc] ON [icc].[Id] = [ic].[CurrencyId]
                           INNER JOIN [dbo].[Balances] AS [b] ON [b].[Id] = [it].[DestinationId] AND [b].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [bc] ON [bc].[Id] = [b].[CurrencyId]
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync(
            sql,
            _mapIncomeCategoryModel,
            new { workspaceId },
            splitOn: "Id,Id,Id,Id");
        return models.Select(x => MapToApplicationModel(x)!).ToArray();
    }

    public async Task<IncomeTransactionModel?> GetById(Guid incomeTransactionId, Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1)
                               [it].[Id], [it].[Date], [it].[AmountInSourceCurrency], [it].[AmountInDestinationCurrency], [it].[Tags],
                               [ic].[Id], [ic].[Name],
                                   [icc].[Id], [icc].[Name], [icc].[Code], [icc].[Symbol],
                               [b].[Id], [b].[Name], [b].[Amount],
                                   [bc].[Id], [bc].[Name], [bc].[Code], [bc].[Symbol]
                           FROM [dbo].[IncomeTransactions] AS [it]
                           INNER JOIN [dbo].[IncomeCategories] AS [ic] ON [ic].[Id] = [it].[SourceId] AND [ic].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [icc] ON [icc].[Id] = [ic].[CurrencyId]
                           INNER JOIN [dbo].[Balances] AS [b] ON [b].[Id] = [it].[DestinationId] AND [b].[WorkspaceId] = @workspaceId
                           INNER JOIN [dbo].[Currencies] AS [bc] ON [bc].[Id] = [b].[CurrencyId]
                           WHERE [it].[Id] = @incomeTransactionId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync(
            sql,
            _mapIncomeCategoryModel,
            new { incomeTransactionId, workspaceId },
            splitOn: "Id,Id,Id,Id");
        return MapToApplicationModel(models.SingleOrDefault());
    }
    
    private readonly Func<IncomeTransactionTableEntry, IncomeCategoryModel, CurrencyModel, BalanceModel, CurrencyModel, IncomeTransactionTableEntry> _mapIncomeCategoryModel =
        (it, ic, icc, b, bc) => it with
        {
            Source = ic with { Currency = icc },
            Destination = b with { Currency = bc }
        };
    
    private static IncomeTransactionModel? MapToApplicationModel(IncomeTransactionTableEntry? tableEntry) =>
        tableEntry is not null
            ? new IncomeTransactionModel(
                tableEntry.Id,
                tableEntry.Date,
                tableEntry.Source,
                tableEntry.AmountInSourceCurrency,
                tableEntry.Destination,
                tableEntry.AmountInDestinationCurrency,
                tableEntry.Tags.Split(','))
            : null;
    
    private sealed record IncomeTransactionTableEntry(
        Guid Id,
        DateOnly Date,
        decimal AmountInSourceCurrency,
        decimal AmountInDestinationCurrency,
        string Tags)
    {
        public required IncomeCategoryModel Source { get; init; }
        public required BalanceModel Destination { get; init; }
    }
}