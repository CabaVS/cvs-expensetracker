using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IBalanceWriteRepository
{
    Task<Balance?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default);
    Task<Guid> Create(Balance balance, Guid workspaceId, CancellationToken ct = default);
}