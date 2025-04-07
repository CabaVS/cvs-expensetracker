using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransactionRepository(ApplicationDbContext dbContext) : ITransactionRepository
{
    public Task<Guid> AddAsync(Transaction transaction, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = TransactionEf.FromDomain(transaction, workspaceId);
        
        EntityEntry<TransactionEf> added = dbContext.Transactions.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }
}
