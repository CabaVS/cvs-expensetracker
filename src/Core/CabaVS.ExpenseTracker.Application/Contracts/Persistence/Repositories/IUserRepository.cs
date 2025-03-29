using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}
