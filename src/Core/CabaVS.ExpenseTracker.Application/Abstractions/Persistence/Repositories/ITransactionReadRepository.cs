using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ITransactionReadRepository
{
    Task<TransactionModel<,>[]> GetAll<TSource, TDestination>(Guid workspaceId, CancellationToken ct = default);
    Task<TransactionModel?> GetById(Guid transactionId, Guid workspaceId, CancellationToken ct = default);
}