using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;

namespace CabaVS.ExpenseTracker.IntegrationTests.Injected;

internal sealed class CurrentUserAccessorInjected : ICurrentUserAccessor
{
    public static Guid AuthenticatedUserId { get; } = new("FE8D6062-625B-43A3-8165-4B091EAC0327");
    
    public Task<AuthenticatedUserModel?> GetCurrentUser(CancellationToken cancellationToken = default) => 
        Task.FromResult(new AuthenticatedUserModel(AuthenticatedUserId, true))!;
}
