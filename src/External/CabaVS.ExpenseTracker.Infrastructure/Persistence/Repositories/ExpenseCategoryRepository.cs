using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using ExpenseCategoryDomain = CabaVS.ExpenseTracker.Domain.Entities.ExpenseCategory;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseCategoryRepository(
    ApplicationDbContext dbContext) : IExpenseCategoryRepository
{
    public async Task<ExpenseCategoryDomain?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = await dbContext.ExpenseCategories
            .AsNoTracking()
            .Include(ec => ec.Currency)
            .Where(ec => ec.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(ec => ec.Id == id, ct);

        return entity?.ToDomain();
    }

    public async Task<Guid> Create(ExpenseCategoryDomain expenseCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseCategory.FromDomain(expenseCategory, workspaceId);

        var added = await dbContext.ExpenseCategories.AddAsync(entity, ct);

        return added.Entity.Id;
    }

    public Task Delete(ExpenseCategoryDomain expenseCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseCategory.FromDomain(expenseCategory, workspaceId);

        dbContext.ExpenseCategories.Remove(entity);

        return Task.CompletedTask;
    }
}