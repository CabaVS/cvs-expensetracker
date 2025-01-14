using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceReadRepository(ApplicationDbContext dbContext) : IWorkspaceReadRepository
{
    public async Task<WorkspaceModel[]> GetWorkspaces(Guid userId, CancellationToken cancellationToken = default)
    {
        WorkspaceModel[] models = await dbContext.UserWorkspaces
            .Where(uw => uw.UserId == userId)
            .Select(uw => new WorkspaceModel(uw.Workspace.Id, uw.Workspace.Name, uw.IsAdmin))
            .ToArrayAsync(cancellationToken);
        return models;
    }

    public async Task<WorkspaceModel?> GetWorkspaceById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        WorkspaceModel? model = await dbContext.UserWorkspaces
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .Select(uw => new WorkspaceModel(uw.Workspace.Id, uw.Workspace.Name, uw.IsAdmin))
            .SingleOrDefaultAsync(cancellationToken);
        return model;
    }
    
    public async Task<bool> IsAdmin(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        var isAdmin = await dbContext.UserWorkspaces
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .Where(uw => uw.IsAdmin)
            .AnyAsync(cancellationToken);
        return isAdmin;
    }

    public async Task<bool> IsMemberOfWorkspace(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        var isMemberOfWorkspace = await dbContext.UserWorkspaces
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .AnyAsync(cancellationToken);
        return isMemberOfWorkspace;
    }
}
