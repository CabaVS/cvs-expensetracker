using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class UserReadRepository(SqlConnectionFactory sqlConnectionFactory) : IUserReadRepository
{
    public async Task<bool> HasAccessToWorkspace(Guid userId, Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT COUNT(1) FROM [dbo].[UserWorkspaces]
                           WHERE [UserId] = @userId AND [WorkspaceId] = @workspaceId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var isExists = await connection.ExecuteScalarAsync<bool>(sql, new { userId, workspaceId });
        return isExists;
    }

    public async Task<bool> HasAdminAccessToWorkspace(Guid userId, Guid workspaceId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT COUNT(1) FROM [dbo].[UserWorkspaces]
                           WHERE [UserId] = @userId AND [WorkspaceId] = @workspaceId AND [IsAdmin] = 1
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var isExists = await connection.ExecuteScalarAsync<bool>(sql, new { userId, workspaceId });
        return isExists;
    }
}