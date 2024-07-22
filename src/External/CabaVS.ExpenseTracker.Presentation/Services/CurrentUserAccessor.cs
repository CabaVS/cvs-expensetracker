using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation.Services;

internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Task<AuthorizedUserModel> Get(CancellationToken ct = default)
    {
        // TODO: Implement actual logic instead of hardcoded model
        var claims = httpContextAccessor.HttpContext!.User.Claims.ToArray();
        
        return Task.FromResult(
            new AuthorizedUserModel(
                Guid.Parse(claims.Single(x => x.Type == "UserId").Value),
                claims.Single(x => x.Type == "FullName").Value,
                bool.Parse(claims.Single(x => x.Type == "IsAdmin").Value)));
    }

    public async Task<Guid> GetId(CancellationToken ct = default) => (await Get(ct)).Id;
}