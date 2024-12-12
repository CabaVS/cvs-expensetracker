using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CurrencyReadRepository(ApplicationDbContext dbContext) : ICurrencyReadRepository
{
    public async Task<CurrencyModel[]> GetAll(CancellationToken cancellationToken = default)
    {
        var allCurrencies = await dbContext.Currencies
            .Select(c => new CurrencyModel(c.Id, c.Name, c.Code, c.Symbol))
            .ToArrayAsync(cancellationToken);
        return allCurrencies;
    }

    public async Task<CurrencyModel?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var currency = await dbContext.Currencies
            .Where(c => c.Id == id)
            .Select(c => new CurrencyModel(c.Id, c.Name, c.Code, c.Symbol))
            .FirstOrDefaultAsync(cancellationToken);
        return currency;
    }
}