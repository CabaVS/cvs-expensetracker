using CabaVS.ExpenseTracker.Application.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

public interface ICurrentUserAccessor
{
    Task<AuthenticatedUserModel?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}
