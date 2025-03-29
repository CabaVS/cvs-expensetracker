using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IWorkspaceRepository
{
    Task<Guid> CreateAsync(Workspace workspace, CancellationToken cancellationToken = default);
}
