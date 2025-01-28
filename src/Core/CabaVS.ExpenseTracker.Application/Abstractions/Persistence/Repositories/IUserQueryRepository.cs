namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IUserQueryRepository
{
    Task<bool> IsExistAsync(Guid userId, CancellationToken cancellationToken = default);
}
