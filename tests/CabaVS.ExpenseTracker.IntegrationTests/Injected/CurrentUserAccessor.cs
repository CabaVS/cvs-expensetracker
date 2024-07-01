using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.IntegrationTests.FakeData;

namespace CabaVS.ExpenseTracker.IntegrationTests.Injected;

internal sealed class CurrentUserAccessorInjected : ICurrentUserAccessor
{ 
    public Task<AuthorizedUserModel> Get(CancellationToken ct = default) => Task.FromResult(AuthorizedUser);
    public Task<Guid> GetId(CancellationToken ct = default) => Task.FromResult(AuthorizedUser.Id);

    public static readonly AuthorizedUserModel AuthorizedUser = new AuthorizedUserModelFaker();
}