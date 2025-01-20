using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Models;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ExpenseCategoryReadRepository(ApplicationDbContext dbContext) : IExpenseCategoryReadRepository
{
    public async Task<ExpenseCategoryModel[]> GetAll(Guid workspaceId, CancellationToken cancellationToken) =>
        await dbContext.ExpenseCategories
            .Where(ec => ec.WorkspaceId == workspaceId)
            .Select(ExpenseCategory.ProjectToModel())
            .ToArrayAsync(cancellationToken);

    public async Task<ExpenseCategoryModel?> GetById(Guid workspaceId, Guid expenseCategoryId, CancellationToken cancellationToken) =>
        await dbContext.ExpenseCategories
            .Where(ec => ec.WorkspaceId == workspaceId)
            .Where(ec => ec.Id == expenseCategoryId)
            .Select(ExpenseCategory.ProjectToModel())
            .FirstOrDefaultAsync(cancellationToken);
}
