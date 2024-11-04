using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IUserRepository
{
    Task<Guid> Create(User user, CancellationToken cancellationToken = default);
}