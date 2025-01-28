using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CurrencyQueryRepository(ApplicationDbContext dbContext) : ICurrencyQueryRepository
{
    public async Task<CurrencyModel[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        CurrencyModel[] allModels = await dbContext.Currencies
            .Select(ProjectToModel)
            .ToArrayAsync(cancellationToken);
        return allModels;
    }

    private static Expression<Func<Currency, CurrencyModel>> ProjectToModel =>
        x => new CurrencyModel(
            x.Id, x.Name, x.Code, x.Symbol);
}
