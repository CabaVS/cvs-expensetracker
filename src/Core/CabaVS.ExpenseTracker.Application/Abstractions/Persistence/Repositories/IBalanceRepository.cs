namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IBalanceRepository
{
    public Task<Guid> Create(Guid workspaceId, Domain.Entities.Balance balance, CancellationToken cancellationToken);
}