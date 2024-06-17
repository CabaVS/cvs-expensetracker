using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeTransactionReadRepository
{
    Task<IncomeTransactionModel[]> GetAll(Guid workspaceId, CancellationToken ct = default);
    Task<IncomeTransactionModel?> GetById(Guid incomeTransactionId, Guid workspaceId, CancellationToken ct = default);
}