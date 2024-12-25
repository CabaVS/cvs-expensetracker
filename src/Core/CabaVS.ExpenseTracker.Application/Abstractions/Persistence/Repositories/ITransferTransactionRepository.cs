namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ITransferTransactionRepository
{
    Task<Guid> Create(Domain.Entities.TransferTransaction transferTransaction, CancellationToken cancellationToken = default);
}