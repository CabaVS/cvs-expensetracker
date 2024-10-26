using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

public interface ICurrentUserAccessor
{
    Task<AuthenticatedUserModel?> GetCurrentUser(CancellationToken cancellationToken = default);
}