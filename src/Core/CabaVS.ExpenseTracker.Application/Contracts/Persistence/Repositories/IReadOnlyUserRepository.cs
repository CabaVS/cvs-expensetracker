using CabaVS.ExpenseTracker.Application.Features.Users.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyUserRepository
{
    Task<UserModel?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}
