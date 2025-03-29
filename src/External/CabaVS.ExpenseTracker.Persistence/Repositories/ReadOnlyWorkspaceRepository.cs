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
            SELECT w.[Id], w.[Name], wm.[Id], wm.[IsAdmin], u.[Id] AS UserId, u.[UserName]
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

        var lookup = new Dictionary<Guid, WorkspaceDetailsDapperModel>(1);
        _ = await connection.QueryAsync<WorkspaceDetailsDapperModel, WorkspaceMemberModel, WorkspaceDetailsDapperModel>(
            sql,
            (workspace, member) =>
            {
                if (!lookup.TryGetValue(workspace.Id, out WorkspaceDetailsDapperModel? currentWorkspace))
                {
                    currentWorkspace = workspace;
                    lookup.Add(currentWorkspace.Id, currentWorkspace);
                }

                currentWorkspace.Members.Add(member);
                return currentWorkspace;
            }, 
            new { UserId = userId, WorkspaceId = workspaceId },
            splitOn: "Id");
        
        return lookup.Values.FirstOrDefault() is { } workspaceDetails
            ? new WorkspaceDetailsModel(workspaceDetails.Id, workspaceDetails.Name, [.. workspaceDetails.Members])
            : null;
    }
    
    internal sealed class WorkspaceDetailsDapperModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WorkspaceMemberModel> Members { get; set; } = [];
    }
}
