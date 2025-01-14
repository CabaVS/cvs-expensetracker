using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransferTransactionRepository(ApplicationDbContext dbContext) : ITransferTransactionRepository
{
    public async Task<Guid> Create(Domain.Entities.TransferTransaction transferTransaction, CancellationToken cancellationToken = default)
    {
        var entityToCreate = TransferTransaction.ConvertFromDomain(transferTransaction);
        
        EntityEntry<TransferTransaction> added = await dbContext.TransferTransactions.AddAsync(entityToCreate, cancellationToken);
        
        return added.Entity.Id;
    }
}
