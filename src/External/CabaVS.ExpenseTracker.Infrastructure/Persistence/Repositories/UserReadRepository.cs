using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class UserReadRepository(ApplicationDbContext dbContext) : IUserReadRepository
{
    public async Task<bool> HasAccessToWorkspace(Guid userId, Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.UserWorkspaces
            .Where(uw => uw.UserId == userId)
            .Where(uw => uw.WorkspaceId == workspaceId)
            .AnyAsync(ct);
    }

    public async Task<bool> HasAdminAccessToWorkspace(Guid userId, Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.UserWorkspaces
            .Where(uw => uw.UserId == userId)
            .Where(uw => uw.WorkspaceId == workspaceId)
            .Where(uw => uw.IsAdmin)
            .AnyAsync(ct);
    }
}