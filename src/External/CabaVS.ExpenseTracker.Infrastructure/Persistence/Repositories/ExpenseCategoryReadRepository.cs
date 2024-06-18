using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseCategoryReadRepository(
    SqlConnectionFactory sqlConnectionFactory) : IExpenseCategoryReadRepository
{
    public async Task<ExpenseCategoryModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT [ic].[Id], [ic].[Name], [c].[Id], [c].[Name], [c].[Code], [c].[Symbol] FROM [dbo].[ExpenseCategories] AS [ic]
                           INNER JOIN [dbo].[Currencies] AS [c] ON [c].[Id] = [ic].[CurrencyId]
                           WHERE [ic].[WorkspaceId] = @workspaceId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync(
            sql,
            _mapExpenseCategoryModel,
            new { workspaceId });
        return models.ToArray();
    }

    public async Task<ExpenseCategoryModel?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1) [ic].[Id], [ic].[Name], [c].[Id], [c].[Name], [c].[Code], [c].[Symbol] FROM [dbo].[ExpenseCategories] AS [ic]
                           INNER JOIN [dbo].[Currencies] AS [c] ON [c].[Id] = [ic].[CurrencyId]
                           WHERE [ic].[Id] = @id AND [ic].[WorkspaceId] = @workspaceId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync(
            sql,
            _mapExpenseCategoryModel,
            new { id, workspaceId });
        return models.SingleOrDefault();
    }
    
    private readonly Func<ExpenseCategoryModel, CurrencyModel, ExpenseCategoryModel> _mapExpenseCategoryModel =
        (b, c) => b with { Currency = c };
}