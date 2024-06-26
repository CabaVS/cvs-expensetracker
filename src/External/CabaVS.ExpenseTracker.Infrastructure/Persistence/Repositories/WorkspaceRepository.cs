using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainWorkspace = CabaVS.ExpenseTracker.Domain.Entities.Workspace;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceRepository(ApplicationDbContext dbContext) : IWorkspaceRepository
{
    public async Task<DomainWorkspace?> GetById(Guid workspaceId, Guid userId, CancellationToken ct = default)
    {
        var entity = await dbContext.Workspaces
            .AsNoTracking()
            .Include(w =>
                w.UserWorkspaces
                    .Where(uw => uw.UserId == userId))
            .FirstOrDefaultAsync(w => w.Id == workspaceId, ct);
        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainWorkspace workspace, Guid userId, CancellationToken ct = default)
    {
        var entity = Workspace.FromDomain(workspace, userId);

        var added = await dbContext.Workspaces.AddAsync(entity, ct);

        return added.Entity.Id;
    }

    public Task Update(DomainWorkspace workspace, Guid userId, CancellationToken ct = default)
    {
        var entity = Workspace.FromDomain(workspace, userId);

        _ = dbContext.Workspaces.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task Delete(DomainWorkspace workspace, Guid userId, CancellationToken ct = default)
    {
        var entity = Workspace.FromDomain(workspace, userId);
        
        _ = dbContext.Workspaces.Remove(entity);
        
        return Task.CompletedTask;
    }
}