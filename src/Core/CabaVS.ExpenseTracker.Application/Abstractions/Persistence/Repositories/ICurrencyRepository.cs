using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetById(Guid id, CancellationToken ct = default);
    Task<Guid> Create(Currency currency, CancellationToken ct = default);
    Task Update(Currency currency, CancellationToken ct = default);
    Task Delete(Currency currency, CancellationToken ct = default);
}