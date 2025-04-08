using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransactionRepository(ApplicationDbContext dbContext) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid transactionId, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        TransactionEf? transaction = await dbContext.Transactions
            .AsNoTracking()
            .Include(t => t.SourceBalance)
            .ThenInclude(x => x!.Currency)
            .Include(t => t.SourceCategory)
            .ThenInclude(x => x!.Currency)
            .Include(t => t.DestinationBalance)
            .ThenInclude(x => x!.Currency)
            .Include(t => t.DestinationCategory)
            .ThenInclude(x => x!.Currency)
            .Where(t => t.Id == transactionId)
            .Where(t => t.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(cancellationToken);
        return transaction?.MapToDomain();
    }

    public Task<Guid> AddAsync(Transaction transaction, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = TransactionEf.FromDomain(transaction, workspaceId);
        
        EntityEntry<TransactionEf> added = dbContext.Transactions.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }

    public Task RemoveAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        var entity = new TransactionEf { Id = transaction.Id };
        
        dbContext.Transactions.Remove(entity);
        
        return Task.CompletedTask;
    }
}
