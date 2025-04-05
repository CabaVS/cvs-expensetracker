using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetByIdAsync(Guid currencyId, CancellationToken cancellationToken = default);
}
