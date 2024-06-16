using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeCategoryReadRepository(
    SqlConnectionFactory sqlConnectionFactory) : IIncomeCategoryReadRepository
{
    public async Task<IncomeCategoryModel?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1) [ic].[Id], [ic].[Name], [c].[Id], [c].[Name], [c].[Code], [c].[Symbol] FROM [dbo].[IncomeCategories] AS [ic]
                           INNER JOIN [dbo].[Currencies] AS [c] ON [c].[Id] = [ic].[CurrencyId]
                           WHERE [ic].[Id] = @id AND [ic].[WorkspaceId] = @workspaceId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync<IncomeCategoryModel, CurrencyModel, IncomeCategoryModel>(
            sql,
            _mapBalanceModelWithCurrency,
            new { id, workspaceId });
        return models.SingleOrDefault();
    }
    
    private readonly Func<IncomeCategoryModel, CurrencyModel, IncomeCategoryModel> _mapBalanceModelWithCurrency =
        (b, c) => b with { Currency = c };
}