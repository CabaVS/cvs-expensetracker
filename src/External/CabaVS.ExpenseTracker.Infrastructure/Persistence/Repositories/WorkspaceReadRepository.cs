using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceReadRepository(SqlConnectionFactory sqlConnectionFactory) : IWorkspaceReadRepository
{
    public async Task<WorkspaceModel?> GetById(Guid id, Guid userId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1) [w].[Id], [w].[Name] FROM [dbo].[Workspaces] AS [w]
                           LEFT JOIN [dbo].[UserWorkspaces] AS [uw] ON [uw].[WorkspaceId] = [w].[Id]
                           WHERE [w].[Id] = @id
                           AND [uw].[UserId] = @userId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var model = await connection.QueryFirstOrDefaultAsync<WorkspaceModel>(sql, new { id, userId });
        return model;
    }
}