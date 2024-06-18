using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IExpenseTransactionReadRepository
{
    Task<ExpenseTransactionModel[]> GetAll(Guid workspaceId, CancellationToken ct = default);
    Task<ExpenseTransactionModel?> GetById(Guid expenseTransactionId, Guid workspaceId, CancellationToken ct = default);
}