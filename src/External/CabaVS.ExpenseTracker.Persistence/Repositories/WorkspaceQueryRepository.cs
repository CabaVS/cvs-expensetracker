using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceQueryRepository(ApplicationDbContext dbContext) : IWorkspaceQueryRepository
{
    public async Task<WorkspaceModel[]> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        WorkspaceModel[] models = await dbContext.UserWorkspaces
            .Where(x => x.UserId == userId)
            .Select(ProjectToModel)
            .ToArrayAsync(cancellationToken);
        return models;
    }

    public async Task<WorkspaceModel?> GetByIdAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        WorkspaceModel? model = await dbContext.UserWorkspaces
            .Where(x => x.UserId == userId)
            .Where(x => x.WorkspaceId == workspaceId)
            .Select(ProjectToModel)
            .FirstOrDefaultAsync(cancellationToken);
        return model;
    }

    public async Task<bool> IsMemberOfWorkspaceAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var isMember = await dbContext.UserWorkspaces
            .Where(x => x.UserId == userId)
            .Where(x => x.WorkspaceId == workspaceId)
            .AnyAsync(cancellationToken);
        return isMember;
    }

    public async Task<bool> IsAdminOfWorkspaceAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var isAdmin = await dbContext.UserWorkspaces
            .Where(x => x.UserId == userId)
            .Where(x => x.WorkspaceId == workspaceId)
            .Select(x => x.IsAdmin)
            .FirstOrDefaultAsync(cancellationToken);
        return isAdmin;
    }
    
    private static Expression<Func<UserWorkspace, WorkspaceModel>> ProjectToModel =>
        x => new WorkspaceModel(
            x.Workspace.Id, x.Workspace.Name, x.IsAdmin);
}
