using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ReadOnlyBalanceRepository(ISqlConnectionFactory connectionFactory) : IReadOnlyBalanceRepository
{
    public async Task<BalanceModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT
                b.[Id], 
                b.[Name],
                b.[Amount],
                c.[Id] AS CurrencyId, 
                c.[Code]
            FROM [dbo].[Balances] b
            INNER JOIN [dbo].[Currencies] c ON b.[CurrencyId] = c.[Id]
            WHERE b.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<BalanceModel> balances = await connection.QueryAsync<BalanceDapperModel, CurrencySlimDapperModel, BalanceModel>(
            sql,
            (temp, currency) => new BalanceModel(temp.Id, temp.Name, temp.Amount, currency.ToCurrencySlimModel()),
            new { WorkspaceId = workspaceId },
            splitOn: "CurrencyId");
        return [.. balances];
    }

    public async Task<BalanceModel?> GetDetailsAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT TOP(1)
                b.[Id], 
                b.[Name],
                b.[Amount],
                c.[Id] AS CurrencyId, 
                c.[Code]
            FROM [dbo].[Balances] b
            INNER JOIN [dbo].[Currencies] c ON b.[CurrencyId] = c.[Id]
            WHERE b.[Id] = @BalanceId AND b.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<BalanceModel> balance = await connection.QueryAsync<BalanceDapperModel, CurrencySlimDapperModel, BalanceModel>(
            sql,
            (temp, currency) => new BalanceModel(temp.Id, temp.Name, temp.Amount, currency.ToCurrencySlimModel()),
            new { WorkspaceId = workspaceId, BalanceId = balanceId },
            splitOn: "CurrencyId");
        return balance.FirstOrDefault();
    }
    
    private sealed record BalanceDapperModel(Guid Id, string Name, decimal Amount);

    private sealed record CurrencySlimDapperModel(Guid CurrencyId, string Code)
    {
        public CurrencySlimModel ToCurrencySlimModel() => new(CurrencyId, Code);
    }
}
