using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceRepository(ApplicationDbContext dbContext) : IWorkspaceRepository
{
    public async Task<Domain.Entities.Workspace?> GetById(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        Workspace? workspace = await dbContext.UserWorkspaces
            .AsNoTracking()
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .Select(uw => uw.Workspace)
            .SingleOrDefaultAsync(cancellationToken);

        Domain.Entities.Workspace? converted = workspace?.ConvertToDomain();
        return converted;
    }

    public async Task<Guid> Create(Domain.Entities.Workspace workspace, CancellationToken cancellationToken = default)
    {
        var converted = Workspace.ConvertFromDomain(workspace);
        
        EntityEntry<Workspace> added = await dbContext.Workspaces.AddAsync(converted, cancellationToken);
        return added.Entity.Id;
    }

    public Task Update(Domain.Entities.Workspace workspace, CancellationToken cancellationToken = default)
    {
        var converted = Workspace.ConvertFromDomain(workspace);
        
        _ = dbContext.Workspaces.Update(converted);
        return Task.CompletedTask;
    }

    public Task Delete(Domain.Entities.Workspace workspace, CancellationToken cancellationToken = default)
    {
        var converted = Workspace.ConvertFromDomain(workspace);
        
        _ = dbContext.Workspaces.Remove(converted);
        return Task.CompletedTask;
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

    public async Task UnregisterUser(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        UserWorkspace userWorkspace = await dbContext.UserWorkspaces
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .SingleAsync(cancellationToken);
        
        dbContext.UserWorkspaces.Remove(userWorkspace);
    }

    public async Task MakeAnAdmin(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default)
    {
        UserWorkspace userWorkspace = await dbContext.UserWorkspaces
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.UserId == userId)
            .SingleAsync(cancellationToken);
        
        userWorkspace.IsAdmin = true;
        
        dbContext.UserWorkspaces.Update(userWorkspace);
    }
}
