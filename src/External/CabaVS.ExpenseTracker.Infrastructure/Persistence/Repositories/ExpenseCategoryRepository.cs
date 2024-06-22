using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainExpenseCategory = CabaVS.ExpenseTracker.Domain.Entities.ExpenseCategory;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseCategoryRepository(ApplicationDbContext dbContext) : IExpenseCategoryRepository
{
    public async Task<DomainExpenseCategory?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = await dbContext.ExpenseCategories
            .AsNoTracking()
            .Include(ec => ec.Currency)
            .Where(ec => ec.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(ec => ec.Id == id, ct);

        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseCategory.FromDomain(expenseCategory, workspaceId);

        var added = await dbContext.ExpenseCategories.AddAsync(entity, ct);

        return added.Entity.Id;
    }

    public Task Update(DomainExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseCategory.FromDomain(expenseCategory, workspaceId);

        _ = dbContext.ExpenseCategories.Update(entity);

        return Task.CompletedTask;
    }

    public Task Delete(DomainExpenseCategory expenseCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = ExpenseCategory.FromDomain(expenseCategory, workspaceId);

        _ = dbContext.ExpenseCategories.Remove(entity);

        return Task.CompletedTask;
    }
}