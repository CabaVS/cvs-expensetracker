using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransferTransactionRepository(ApplicationDbContext dbContext) : ITransferTransactionRepository
{
    public async Task<Guid> Create(Domain.Entities.TransferTransaction transferTransaction, CancellationToken cancellationToken = default)
    {
        var entityToCreate = TransferTransaction.ConvertFromDomain(transferTransaction);
        
        var added = await dbContext.TransferTransactions.AddAsync(entityToCreate, cancellationToken);
        
        return added.Entity.Id;
    }
}