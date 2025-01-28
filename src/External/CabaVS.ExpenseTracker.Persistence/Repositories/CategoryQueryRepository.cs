using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CategoryQueryRepository(ApplicationDbContext dbContext) : ICategoryQueryRepository
{
    public async Task<CategoryModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        CategoryModel[] allModels = await dbContext.Categories
            .Where(x => x.WorkspaceId == workspaceId)
            .Select(ProjectToModel)
            .ToArrayAsync(cancellationToken);
        return allModels;
    }

    public async Task<CategoryModel?> GetByIdAsync(Guid workspaceId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        CategoryModel? model = await dbContext.Categories
            .Where(x => x.WorkspaceId == workspaceId)
            .Where(x => x.Id == categoryId)
            .Select(ProjectToModel)
            .FirstOrDefaultAsync(cancellationToken);
        return model;
    }

    private static Expression<Func<Category, CategoryModel>> ProjectToModel =>
        x => new CategoryModel(
            x.Id, x.Name, x.Type, new CurrencyModel(
                x.Currency.Id, x.Currency.Name, x.Currency.Code, x.Currency.Symbol));
}
