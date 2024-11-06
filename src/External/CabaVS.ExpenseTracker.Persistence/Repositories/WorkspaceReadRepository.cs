using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceReadRepository(ApplicationDbContext dbContext) : IWorkspaceReadRepository
{
    public async Task<WorkspaceModel[]> GetWorkspaces(Guid userId, CancellationToken cancellationToken = default)
    {
        var models = await dbContext.UserWorkspaces
            .Where(uw => uw.UserId == userId)
            .Select(uw => new WorkspaceModel(uw.Workspace.Id, uw.Workspace.Name, uw.IsAdmin))
            .ToArrayAsync(cancellationToken);
        return models;
    }

    public async Task<WorkspaceModel?> GetWorkspaceById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        var model = await dbContext.UserWorkspaces
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .Select(uw => new WorkspaceModel(uw.Workspace.Id, uw.Workspace.Name, uw.IsAdmin))
            .SingleOrDefaultAsync(cancellationToken);
        return model;
    }
}