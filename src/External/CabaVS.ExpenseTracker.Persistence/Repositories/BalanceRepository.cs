using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceRepository(ApplicationDbContext dbContext) : IBalanceRepository
{
    public async Task<Domain.Entities.Balance?> GetById(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken)
    {
        var balance = await dbContext.Balances
            .AsNoTracking()
            .Include(b => b.Currency)
            .Where(b => b.Id == balanceId)
            .Where(b => b.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        return balance?.ConvertToDomain();
    }

    public async Task<Guid> Create(Guid workspaceId, Domain.Entities.Balance balance, CancellationToken cancellationToken)
    {
        var balanceToCreate = Balance.ConvertFromDomain(balance, workspaceId);
        
        var addedEntityEntry = await dbContext.Balances.AddAsync(balanceToCreate, cancellationToken);
        
        return addedEntityEntry.Entity.Id;
    }

    public Task Update(Guid workspaceId, Domain.Entities.Balance balance, CancellationToken cancellationToken)
    {
        var balanceToUpdate = Balance.ConvertFromDomain(balance, workspaceId);
        
        _ = dbContext.Balances.Update(balanceToUpdate);
        
        return Task.CompletedTask;
    }

    public async Task Delete(Guid workspaceId, Domain.Entities.Balance balance, CancellationToken cancellationToken)
    {
        var balanceToDelete = await dbContext.Balances
            .Where(b => b.Id == balance.Id)
            .Where(b => b.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        
        _ = dbContext.Balances.Remove(
            balanceToDelete ?? throw new InvalidOperationException("Balance for deletion is not found."));
    }
}