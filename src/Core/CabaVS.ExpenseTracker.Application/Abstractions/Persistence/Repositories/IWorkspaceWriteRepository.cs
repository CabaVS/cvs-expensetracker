using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IWorkspaceWriteRepository
{
    Task<Guid> Create(Workspace workspace, Guid userId, bool isWorkspaceAdmin = true, CancellationToken ct = default);
}