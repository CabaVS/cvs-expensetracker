using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainCurrency = CabaVS.ExpenseTracker.Domain.Entities.Currency;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class CurrencyWriteRepository(ApplicationDbContext dbContext) : ICurrencyWriteRepository
{
    public async Task<Guid> Create(DomainCurrency currency, CancellationToken ct = default)
    {
        var entity = Currency.FromDomain(currency);

        var added = await dbContext.Currencies.AddAsync(entity, ct);

        return added.Entity.Id;
    }
}