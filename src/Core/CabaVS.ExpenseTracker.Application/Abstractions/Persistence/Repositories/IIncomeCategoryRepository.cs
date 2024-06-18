using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeCategoryRepository
{
    Task<IncomeCategory?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default);
    Task<Guid> Create(IncomeCategory incomeCategory, Guid workspaceId, CancellationToken ct = default);
}