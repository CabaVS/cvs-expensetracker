using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Persistence.Dapper;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ReadOnlyCategoryRepository(ISqlConnectionFactory connectionFactory) : IReadOnlyCategoryRepository
{
    public async Task<CategoryModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT
                cat.[Id], 
                cat.[Name],
                cat.[Type],
                c.[Id] AS CurrencyId, 
                c.[Code]
            FROM [dbo].[Categories] cat
            INNER JOIN [dbo].[Currencies] c ON cat.[CurrencyId] = c.[Id]
            WHERE cat.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<CategoryModel> categories = await connection.QueryAsync<CategoryDapperModel, CurrencySlimDapperModel, CategoryModel>(
            sql,
            (temp, currency) => new CategoryModel(temp.Id, temp.Name, temp.Type, currency.ToCurrencySlimModel()),
            new { WorkspaceId = workspaceId },
            splitOn: "CurrencyId");
        return [.. categories];
    }

    public async Task<CategoryModel?> GetByIdAsync(Guid workspaceId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT
                cat.[Id], 
                cat.[Name],
                cat.[Type],
                c.[Id] AS CurrencyId, 
                c.[Code]
            FROM [dbo].[Categories] cat
            INNER JOIN [dbo].[Currencies] c ON cat.[CurrencyId] = c.[Id]
            WHERE cat.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<CategoryModel> categories = await connection.QueryAsync<CategoryDapperModel, CurrencySlimDapperModel, CategoryModel>(
            sql,
            (temp, currency) => new CategoryModel(temp.Id, temp.Name, temp.Type, currency.ToCurrencySlimModel()),
            new { WorkspaceId = workspaceId },
            splitOn: "CurrencyId");
        return categories.FirstOrDefault();
    }

    private sealed record CategoryDapperModel(Guid Id, string Name, CategoryType Type);

    private sealed record CurrencySlimDapperModel(Guid CurrencyId, string Code)
    {
        public CurrencySlimModel ToCurrencySlimModel() => new(CurrencyId, Code);
    }
}
