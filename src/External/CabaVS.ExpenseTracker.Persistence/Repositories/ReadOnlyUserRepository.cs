using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Users.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ReadOnlyUserRepository(ISqlConnectionFactory connectionFactory) : IReadOnlyUserRepository
{
    public async Task<UserModel?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT [Id], [UserName], [IsAdmin]
            FROM [dbo].[Users]
            WHERE Id = @UserId 
            """;
        
        UserModel? user = await connection.QueryFirstOrDefaultAsync<UserModel>(sql, new { UserId = userId });
        return user;
    }
}
