using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IBalanceRepository
{
    public Task<Balance?> GetById(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken);
    
    public Task<Guid> Create(Guid workspaceId, Balance balance, CancellationToken cancellationToken);
    public Task Update(Guid workspaceId, Balance balance, CancellationToken cancellationToken);
    public Task Delete(Guid workspaceId, Balance balance, CancellationToken cancellationToken);
}