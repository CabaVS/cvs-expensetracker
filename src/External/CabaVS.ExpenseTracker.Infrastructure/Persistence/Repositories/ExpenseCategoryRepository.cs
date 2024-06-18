using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using ExpenseCategoryDomain = CabaVS.ExpenseTracker.Domain.Entities.ExpenseCategory;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseCategoryRepository(
    ApplicationDbContext dbContext) : IExpenseCategoryRepository
{
    public async Task<Guid> Create(ExpenseCategoryDomain expenseCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseCategory.FromDomain(expenseCategory, workspaceId);

        var added = await dbContext.ExpenseCategories.AddAsync(entity, ct);

        return added.Entity.Id;
    }
}