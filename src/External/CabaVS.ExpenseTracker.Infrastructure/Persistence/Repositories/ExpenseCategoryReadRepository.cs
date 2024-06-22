using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseCategoryReadRepository(ApplicationDbContext dbContext) : IExpenseCategoryReadRepository
{
    public async Task<ExpenseCategoryModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.ExpenseCategories
            .Include(ec => ec.Currency)
            .Where(ec => ec.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<ExpenseCategoryModel?> GetById(Guid expenseCategoryId, Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.ExpenseCategories
            .Include(ec => ec.Currency)
            .Where(ec => ec.WorkspaceId == workspaceId)
            .Where(ec => ec.Id == expenseCategoryId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }
    
    private static readonly Expression<Func<ExpenseCategory, ExpenseCategoryModel>> EntityToModelProjection =
        expenseCategory => new ExpenseCategoryModel(
            expenseCategory.Id, 
            expenseCategory.Name,
            new CurrencyModel(
                expenseCategory.Currency.Id,
                expenseCategory.Currency.Name,
                expenseCategory.Currency.Code,
                expenseCategory.Currency.Symbol));
}