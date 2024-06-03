using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using DomainCurrency = CabaVS.ExpenseTracker.Domain.Entities.Currency;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class CurrencyWriteRepository(ApplicationDbContext dbContext) : ICurrencyWriteRepository
{
    public Task<Guid> Create(DomainCurrency currency, CancellationToken ct = default)
    {
        var entity = Currency.FromDomain(currency);

        var added = dbContext.Currencies.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }
}