using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeCategoryReadRepository(ApplicationDbContext dbContext) : IIncomeCategoryReadRepository
{
    public async Task<IncomeCategoryModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.IncomeCategories
            .Include(ic => ic.Currency)
            .Where(ic => ic.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<IncomeCategoryModel?> GetById(Guid incomeCategoryId, Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.IncomeCategories
            .Include(ic => ic.Currency)
            .Where(ic => ic.Id == incomeCategoryId)
            .Where(ic => ic.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }
    
    private static readonly Expression<Func<IncomeCategory, IncomeCategoryModel>> EntityToModelProjection =
        incomeCategory => new IncomeCategoryModel(
        incomeCategory.Id, 
        incomeCategory.Name,
        new CurrencyModel(
            incomeCategory.Currency.Id,
            incomeCategory.Currency.Name,
            incomeCategory.Currency.Code,
            incomeCategory.Currency.Symbol));
}