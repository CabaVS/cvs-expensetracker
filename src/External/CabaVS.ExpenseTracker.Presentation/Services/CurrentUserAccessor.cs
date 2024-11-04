using System.Security.Claims;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation.Services;

internal sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    private const string CurrentUserKey = "CurrentUser";
    
    public Task<AuthenticatedUserModel?> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null || context.User.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult<AuthenticatedUserModel?>(null);
        }

        if (context.Items[CurrentUserKey] is AuthenticatedUserModel foundUser)
        {
            return Task.FromResult<AuthenticatedUserModel?>(foundUser);
        }
        
        var claims = context.User.Claims.ToArray();
        
        var userId = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out var userIdParsed))
        {
            return Task.FromResult<AuthenticatedUserModel?>(null);
        }
        
        var isAdmin = claims.Any(claim => claim.Type == "CVS:IsAdmin");

        foundUser = new AuthenticatedUserModel(userIdParsed, isAdmin);
        context.Items[CurrentUserKey] = foundUser;
        
        return Task.FromResult(foundUser)!;
    }
}