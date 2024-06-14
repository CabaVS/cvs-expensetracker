using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using Dapper;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceReadRepository(SqlConnectionFactory sqlConnectionFactory) : IWorkspaceReadRepository
{
    public async Task<WorkspaceModel[]> GetAll(Guid userId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT [w].[Id], [w].[Name], [uw].[IsAdmin] FROM [dbo].[Workspaces] AS [w]
                           INNER JOIN [dbo].[UserWorkspaces] AS [uw] ON [uw].[WorkspaceId] = [w].[Id] AND [uw].[UserId] = @userId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var models = await connection.QueryAsync<WorkspaceModel>(sql, new { userId });
        return models.ToArray();
    }

    public async Task<WorkspaceModel?> GetById(Guid workspaceId, Guid userId, CancellationToken ct = default)
    {
        const string sql = """
                           SELECT TOP(1) [w].[Id], [w].[Name], [uw].[IsAdmin] FROM [dbo].[Workspaces] AS [w]
                           INNER JOIN [dbo].[UserWorkspaces] AS [uw] ON [uw].[WorkspaceId] = [w].[Id] AND [uw].[UserId] = @userId
                           WHERE [w].[Id] = @workspaceId
                           """;
        
        using var connection = sqlConnectionFactory.Create();
        var model = await connection.QueryFirstOrDefaultAsync<WorkspaceModel>(sql, new { workspaceId, userId });
        return model;
    }
}