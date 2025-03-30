using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyCurrencyRepository
{
    Task<CurrencyModel[]> GetAllAsync(CancellationToken cancellationToken = default);
}
