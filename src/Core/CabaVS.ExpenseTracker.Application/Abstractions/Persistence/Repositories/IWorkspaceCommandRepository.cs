using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceCommandRepository : IGenericCommandRepository<Workspace>
{
    Task AddUserAsync(Guid workspaceId, Guid userId, bool admin, CancellationToken cancellationToken = default);
}
