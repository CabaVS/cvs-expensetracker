using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICurrencyQueryRepository
{
    Task<CurrencyModel[]> GetAllAsync(CancellationToken cancellationToken = default);
}
