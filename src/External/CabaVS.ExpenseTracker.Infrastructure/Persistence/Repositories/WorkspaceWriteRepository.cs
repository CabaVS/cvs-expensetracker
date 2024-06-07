using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainWorkspace = CabaVS.ExpenseTracker.Domain.Entities.Workspace;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceWriteRepository(ApplicationDbContext dbContext) : IWorkspaceWriteRepository
{
    public async Task<Guid> Create(DomainWorkspace workspace, Guid userId, CancellationToken ct = default)
    {
        var entity = Workspace.FromDomain(workspace, userId);

        var added = await dbContext.Workspaces.AddAsync(entity, ct);

        return added.Entity.Id;
    }
}