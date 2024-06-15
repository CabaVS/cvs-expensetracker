using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainIncomeCategory = CabaVS.ExpenseTracker.Domain.Entities.IncomeCategory;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeCategoryWriteRepository(
    ApplicationDbContext dbContext) : IIncomeCategoryWriteRepository
{
    public async Task<Guid> Create(DomainIncomeCategory incomeCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeCategory.FromDomain(incomeCategory, workspaceId);

        var added = await dbContext.IncomeCategories.AddAsync(entity, ct);

        return added.Entity.Id;
    }
}