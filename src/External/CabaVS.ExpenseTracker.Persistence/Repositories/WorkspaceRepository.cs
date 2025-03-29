using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceRepository(ApplicationDbContext dbContext) : IWorkspaceRepository
{
    public Task<Guid> CreateAsync(Workspace workspace, CancellationToken cancellationToken = default)
    {
        var entity = WorkspaceEf.FromDomain(workspace);
        
        EntityEntry<WorkspaceEf> added = dbContext.Workspaces.Add(entity);
        
        return Task.FromResult(added.Entity.Id);
    }
}
