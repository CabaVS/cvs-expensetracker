using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class WorkspaceCommandRepository(ApplicationDbContext dbContext) :
    GenericCommandRepository<Domain.Entities.Workspace, Workspace>(dbContext),
    IWorkspaceCommandRepository
{
    public async Task AddUserAsync(Guid workspaceId, Guid userId, bool admin, CancellationToken cancellationToken = default)
    {
        var userWorkspace = new UserWorkspace
        {
            WorkspaceId = workspaceId,
            UserId = userId,
            IsAdmin = admin
        };
        
        await DbContext.UserWorkspaces.AddAsync(userWorkspace, cancellationToken);
    }
}
