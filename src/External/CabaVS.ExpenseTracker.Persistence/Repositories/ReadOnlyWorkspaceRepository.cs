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
}
