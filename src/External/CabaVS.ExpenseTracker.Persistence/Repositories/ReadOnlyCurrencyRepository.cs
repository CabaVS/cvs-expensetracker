using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ReadOnlyCurrencyRepository(ISqlConnectionFactory connectionFactory) : IReadOnlyCurrencyRepository
{
    public async Task<CurrencyModel[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT [Id], [Name], [Code], [Symbol]
            FROM [dbo].[Currencies] 
            """;
        
        IEnumerable<CurrencyModel> currencies = await connection.QueryAsync<CurrencyModel>(sql);
        return [.. currencies];
    }
}
