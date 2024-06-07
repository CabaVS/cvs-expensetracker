using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class CurrencyReadRepository(SqlConnectionFactory sqlConnectionFactory) : ICurrencyReadRepository
{
    public async Task<CurrencyModel?> GetById(Guid id, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1) [Id], [Name], [Code], [Symbol] FROM [dbo].[Currencies]
                           WHERE [Id] = @id
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var model = await connection.QueryFirstOrDefaultAsync<CurrencyModel>(sql, new { id });
        return model;
    }
}