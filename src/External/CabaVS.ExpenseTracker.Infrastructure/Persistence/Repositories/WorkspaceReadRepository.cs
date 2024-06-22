using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceReadRepository(ApplicationDbContext dbContext) : IWorkspaceReadRepository
{
    public async Task<WorkspaceModel[]> GetAll(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.UserWorkspaces
            .Include(uw => uw.Workspace)
            .Where(uw => uw.UserId == userId)
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<WorkspaceModel?> GetById(Guid workspaceId, Guid userId, CancellationToken ct = default)
    {
        return await dbContext.UserWorkspaces
            .Include(uw => uw.Workspace)
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }
    
    private static readonly Expression<Func<UserWorkspace, WorkspaceModel>> EntityToModelProjection =
        userWorkspace => new WorkspaceModel(
            userWorkspace.Workspace.Id, 
            userWorkspace.Workspace.Name, 
            userWorkspace.IsAdmin);
}