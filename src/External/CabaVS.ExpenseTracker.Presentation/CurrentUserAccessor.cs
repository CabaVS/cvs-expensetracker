using System.Security.Claims;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation;

internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Guid UserId => 
        Guid.Parse(
            httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new InvalidOperationException("User is not authenticated."));
}
