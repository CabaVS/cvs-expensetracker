using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class CurrencyReadRepository(ApplicationDbContext dbContext) : ICurrencyReadRepository
{
    public async Task<CurrencyModel[]> GetAll(CancellationToken ct = default)
    {
        return await dbContext.Currencies
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<CurrencyModel?> GetById(Guid currencyId, CancellationToken ct = default)
    {
        return await dbContext.Currencies
            .Where(c => c.Id == currencyId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }
    
    private static readonly Expression<Func<Currency, CurrencyModel>> EntityToModelProjection =
        currency => new CurrencyModel(
            currency.Id, 
            currency.Name, 
            currency.Code,
            currency.Symbol);
}