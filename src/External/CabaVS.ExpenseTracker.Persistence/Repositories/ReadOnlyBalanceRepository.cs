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
            SELECT b.[Id], b.[Name]
            FROM [dbo].[Balances] b
            WHERE b.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<BalanceModel> balances = await connection.QueryAsync<BalanceModel>(sql, new { WorkspaceId = workspaceId });
        return [.. balances];
    }

    public async Task<BalanceDetailsModel?> GetDetailsAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT TOP(1)
                b.[Id], 
                b.[Name],
                c.[Id] AS CurrencyId, 
                c.[Code]
            FROM [dbo].[Balances] b
            INNER JOIN [dbo].[Currencies] c ON b.[CurrencyId] = c.[Id]
            WHERE b.[Id] = @BalanceId AND b.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<BalanceDetailsModel> balance = await connection.QueryAsync<TempBalanceDetails, CurrencySlimModel, BalanceDetailsModel>(
            sql,
            (temp, currency) => new BalanceDetailsModel(temp.Id, temp.Name, currency),
            new { WorkspaceId = workspaceId, BalanceId = balanceId },
            splitOn: "CurrencyId");
        return balance.FirstOrDefault();
    }
    
    private sealed record TempBalanceDetails(Guid Id, string Name);
}
