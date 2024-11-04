namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IUserReadRepository
{
    Task<bool> IsExistById(Guid userId, CancellationToken cancellationToken = default);
}