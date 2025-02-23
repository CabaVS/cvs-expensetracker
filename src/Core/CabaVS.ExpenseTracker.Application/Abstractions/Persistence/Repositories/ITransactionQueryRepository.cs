using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ITransactionQueryRepository
{
    Task<TransactionModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<TransactionModel?> GetByIdAsync(Guid workspaceId, Guid transactionId, CancellationToken cancellationToken = default);
    
    Task<TransactionMoneyModel[]> GetTransactionsMoneyAsync(
        Guid workspaceId, TransactionType type, DateOnly from, DateOnly to, 
        CancellationToken cancellationToken = default);
    Task<TransactionMoneyByCategoryModel[]> GetTransactionsMoneyByCategoryAsync(
        Guid workspaceId, TransactionType type, DateOnly from, DateOnly to, 
        CancellationToken cancellationToken = default);
}
