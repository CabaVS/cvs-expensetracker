using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainBalance = CabaVS.ExpenseTracker.Domain.Entities.Balance;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class BalanceRepository(ApplicationDbContext dbContext) : IBalanceRepository
{
    public async Task<DomainBalance?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = await dbContext.Balances
            .AsNoTracking()
            .Include(b => b.Currency)
            .Where(b => b.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainBalance balance, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = Balance.FromDomain(balance, workspaceId);

        var added = await dbContext.Balances.AddAsync(entity, ct);

        return added.Entity.Id;
    }

    public Task Update(DomainBalance balance, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = Balance.FromDomain(balance, workspaceId);

        _ = dbContext.Balances.Update(entity);

        return Task.CompletedTask;
    }
}