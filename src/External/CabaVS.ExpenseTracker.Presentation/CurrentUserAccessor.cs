using System.Security.Claims;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation;

internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Guid? UserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim is not null
                ? Guid.Parse(userIdClaim)
                : null;
        }
    }
}
