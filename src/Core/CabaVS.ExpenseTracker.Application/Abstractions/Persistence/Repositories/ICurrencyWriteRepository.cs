using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICurrencyWriteRepository
{
    Task<Guid> Create(Currency currency, CancellationToken ct = default);
}