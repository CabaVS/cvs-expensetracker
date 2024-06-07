using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

namespace CabaVS.ExpenseTracker.Presentation.Services;

internal sealed class CurrentUserAccessor : ICurrentUserAccessor
{
    public Task<AuthorizedUserModel> Get(CancellationToken ct = default)
    {
        // TODO: Implement actual logic instead of hardcoded model
        return Task.FromResult(
            new AuthorizedUserModel(
                new Guid("59327D2E-2829-43CC-9A9F-4D2B1BDC271A"),
                "Test User",
                true));
    }

    public async Task<Guid> GetId(CancellationToken ct = default) => (await Get(ct)).Id;
}