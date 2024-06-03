namespace CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

public interface ICurrentUserAccessor
{
    Task<AuthorizedUserModel> Get(CancellationToken ct = default);
}