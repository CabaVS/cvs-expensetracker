using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeCategoryWriteRepository
{
    Task<Guid> Create(IncomeCategory incomeCategory, Guid workspaceId, CancellationToken ct = default);
}