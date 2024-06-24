using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

namespace CabaVS.ExpenseTracker.IntegrationTests.Injected;

internal sealed class CurrentUserAccessorInjected(AuthorizedUserModel userModel) : ICurrentUserAccessor
{ 
    public Task<AuthorizedUserModel> Get(CancellationToken ct = default) => Task.FromResult(userModel);

    public Task<Guid> GetId(CancellationToken ct = default) => Task.FromResult(userModel.Id);
}