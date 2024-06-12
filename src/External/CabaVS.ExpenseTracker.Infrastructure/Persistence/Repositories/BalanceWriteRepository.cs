using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainBalance = CabaVS.ExpenseTracker.Domain.Entities.Balance;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class BalanceWriteRepository(ApplicationDbContext dbContext) : IBalanceWriteRepository
{
    public async Task<Guid> Create(DomainBalance balance, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = Balance.FromDomain(balance, workspaceId);

        var added = await dbContext.Balances.AddAsync(entity, ct);

        return added.Entity.Id;
    }
}