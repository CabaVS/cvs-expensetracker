using System.Security.Claims;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation.Services;

internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Task<AuthenticatedUserModel?> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult<AuthenticatedUserModel?>(null);
        }
        
        var claims = httpContextAccessor.HttpContext.User.Claims.ToArray();
        
        var userId = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out var userIdParsed))
        {
            return Task.FromResult<AuthenticatedUserModel?>(null);
        }
        
        var isAdmin = claims.Any(claim => claim.Type == "CVS:IsAdmin");
        
        return Task.FromResult(new AuthenticatedUserModel(userIdParsed, isAdmin))!;
    }
}