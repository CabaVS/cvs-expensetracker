using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICurrencyReadRepository
{
    Task<CurrencyModel?> GetById(Guid id, CancellationToken ct = default);
}