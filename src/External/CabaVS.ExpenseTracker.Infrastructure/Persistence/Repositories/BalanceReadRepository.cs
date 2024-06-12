using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class BalanceReadRepository(SqlConnectionFactory sqlConnectionFactory) : IBalanceReadRepository
{
    public async Task<BalanceModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT [b].[Id], [b].[Name], [b].[Amount], [c].[Id], [c].[Name], [c].[Code], [c].[Symbol] FROM [dbo].[Balances] AS [b]
                           INNER JOIN [dbo].[Currencies] AS [c] ON [c].[Id] = [b].[CurrencyId]
                           WHERE [b].[WorkspaceId] = @workspaceId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync<BalanceModel, CurrencyModel, BalanceModel>(
            sql,
            (b, c) => b with { Currency = c },
            new { workspaceId });
        return models.ToArray();
    }
}