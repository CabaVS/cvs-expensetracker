using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceRepository(ApplicationDbContext dbContext) : IWorkspaceRepository
{
    public async Task<Domain.Entities.Workspace[]> GetAll(Guid userId, CancellationToken cancellationToken = default)
    {
        var workspaces = await dbContext.UserWorkspaces
            .AsNoTracking()
            .Where(uw => uw.UserId == userId)
            .Select(uw => uw.Workspace)
            .ToArrayAsync(cancellationToken);

        var converted = workspaces
            .Select(w => w.ConvertToDomain())
            .ToArray();
        return converted;
    }

    public Task<Domain.Entities.Workspace?> GetById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> Create(Domain.Entities.Workspace workspace, CancellationToken cancellationToken = default)
    {
        var converted = Workspace.ConvertFromDomain(workspace);
        
        var added = await dbContext.Workspaces.AddAsync(converted, cancellationToken);
        return added.Entity.Id;
    }

    public Task Update(Domain.Entities.Workspace workspace, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Domain.Entities.Workspace workspace, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RegisterUser(Guid workspaceId, Guid userId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var userWorkspace = new UserWorkspace
        {
            UserId = userId,
            WorkspaceId = workspaceId,
            IsAdmin = isAdmin
        };
        
        await dbContext.UserWorkspaces.AddAsync(userWorkspace, cancellationToken);
    }

    public Task UnregisterUser(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}