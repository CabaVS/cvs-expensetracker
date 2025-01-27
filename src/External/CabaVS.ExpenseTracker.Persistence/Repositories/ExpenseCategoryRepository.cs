using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ExpenseCategoryRepository(ApplicationDbContext dbContext) : IExpenseCategoryRepository
{
    public async Task<Domain.Entities.ExpenseCategory?> GetByIdAsync(
        Guid workspaceId, Guid expenseCategoryId, CancellationToken cancellationToken)
    {
        ExpenseCategory? expenseCategory = await dbContext.ExpenseCategories
            .AsNoTracking()
            .Include(ec => ec.Currency)
            .Where(ec => ec.WorkspaceId == workspaceId)
            .Where(ec => ec.Id == expenseCategoryId)
            .FirstOrDefaultAsync(cancellationToken);
        return expenseCategory?.ConvertToDomainEntity();
    }

    public async Task<Guid> CreateAsync(
        Guid workspaceId,
        Domain.Entities.ExpenseCategory expenseCategory,
        CancellationToken cancellationToken)
    {
        var expenseCategoryToCreate = ExpenseCategory.ConvertFromDomainEntity(expenseCategory, workspaceId);
        
        EntityEntry<ExpenseCategory> entityEntry = await dbContext.ExpenseCategories.AddAsync(expenseCategoryToCreate, cancellationToken);
        
        return entityEntry.Entity.Id;
    }

    public Task UpdateAsync(
        Guid workspaceId,
        Domain.Entities.ExpenseCategory expenseCategory,
        CancellationToken cancellationToken)
    {
        var expenseCategoryToUpdate = ExpenseCategory.ConvertFromDomainEntity(expenseCategory, workspaceId);
        
        dbContext.ExpenseCategories.Update(expenseCategoryToUpdate);

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(
        Guid workspaceId,
        Domain.Entities.ExpenseCategory expenseCategory,
        CancellationToken cancellationToken)
    {
        ExpenseCategory existingExpenseCategory = await dbContext.ExpenseCategories
                                                      .Where(ec => ec.WorkspaceId == workspaceId)
                                                      .Where(ec => ec.Id == expenseCategory.Id)
                                                      .FirstOrDefaultAsync(cancellationToken) 
                                                  ?? throw new InvalidOperationException("Expense Category not found.");
        
        dbContext.ExpenseCategories.Remove(existingExpenseCategory);
    }
}
