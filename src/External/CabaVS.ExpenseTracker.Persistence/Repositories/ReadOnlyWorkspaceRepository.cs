using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ReadOnlyWorkspaceRepository(ISqlConnectionFactory connectionFactory) : IReadOnlyWorkspaceRepository
{
    public async Task<WorkspaceModel[]> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT [w].[Id], [w].[Name] FROM [dbo].[WorkspaceMembers] AS [wm]
            INNER JOIN [dbo].[Workspaces] AS [w] ON [w].[Id] = [wm].[WorkspaceId]
            WHERE [wm].[UserId] = @UserId 
            """;
        
        IEnumerable<WorkspaceModel> workspaces = await connection.QueryAsync<WorkspaceModel>(sql, new { UserId = userId });
        return [..workspaces];
    }

    public async Task<WorkspaceDetailsModel?> GetDetailsAsync(
        Guid userId, 
        Guid workspaceId,
        CancellationToken cancellationToken)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            """
            SELECT w.[Id], w.[Name], wm.[Id] AS MemberId, wm.[IsAdmin], u.[Id] AS UserId, u.[UserName]
            FROM [dbo].[Workspaces] w
            LEFT JOIN [dbo].[WorkspaceMembers] wm ON w.[Id] = wm.[WorkspaceId]
            LEFT JOIN [dbo].[Users] u ON wm.[UserId] = u.[Id]
            WHERE w.[Id] = @WorkspaceId
            AND EXISTS (
                SELECT 1 
                FROM [dbo].[WorkspaceMembers] 
                WHERE [WorkspaceId] = @WorkspaceId 
                  AND [UserId] = @UserId
            )
            """;
        
        WorkspaceDetailsModel? user = await connection.QueryFirstOrDefaultAsync<WorkspaceDetailsModel>(
            sql, 
            new { UserId = userId, WorkspaceId = workspaceId });
        return user;
    }
}
