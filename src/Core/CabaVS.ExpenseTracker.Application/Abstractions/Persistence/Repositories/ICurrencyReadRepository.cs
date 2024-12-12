using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICurrencyReadRepository
{
    Task<CurrencyModel[]> GetAll(CancellationToken cancellationToken = default);
    Task<CurrencyModel?> GetById(Guid id, CancellationToken cancellationToken = default);
}