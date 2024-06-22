using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainCurrency = CabaVS.ExpenseTracker.Domain.Entities.Currency;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class CurrencyRepository(ApplicationDbContext dbContext) : ICurrencyRepository
{
    public async Task<DomainCurrency?> GetById(Guid id, CancellationToken ct = default)
    {
        var entity = await dbContext.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainCurrency currency, CancellationToken ct = default)
    {
        var entity = Currency.FromDomain(currency);

        var added = await dbContext.Currencies.AddAsync(entity, ct);

        return added.Entity.Id;
    }

    public Task Update(DomainCurrency currency, CancellationToken ct = default)
    {
        var entity = Currency.FromDomain(currency);

        _ = dbContext.Currencies.Update(entity);

        return Task.CompletedTask;
    }

    public Task Delete(DomainCurrency currency, CancellationToken ct = default)
    {
        var entity = Currency.FromDomain(currency);

        _ = dbContext.Currencies.Remove(entity);

        return Task.CompletedTask;
    }
}