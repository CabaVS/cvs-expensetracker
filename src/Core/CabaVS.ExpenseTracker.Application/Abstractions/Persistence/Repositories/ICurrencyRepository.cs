using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetById(Guid id, CancellationToken cancellationToken);
}