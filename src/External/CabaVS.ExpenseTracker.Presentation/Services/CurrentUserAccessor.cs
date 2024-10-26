using System.Security.Claims;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CabaVS.ExpenseTracker.Presentation.Services;

internal sealed class CurrentUserAccessor(
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public Task<AuthenticatedUserModel?> GetCurrentUser(CancellationToken cancellationToken = default)
    {
#if DEBUG
        var testUserId = Guid.Parse(
            configuration["TestUser:Id"]
            ?? throw new InvalidOperationException("TestUser:Id is missing."));
        var testUserIsAdmin = bool.Parse(
            configuration["TestUser:IsAdmin"]
            ?? throw new InvalidOperationException("TestUser:IsAdmin is missing."));
        
        return Task.FromResult(new AuthenticatedUserModel(testUserId, testUserIsAdmin))!;
#endif
        
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
        
        var isAdmin = claims
            .Where(claim => claim.Type == ClaimTypes.Role)
            .Any(claim => claim.Value == "Admin");
        
        return Task.FromResult(new AuthenticatedUserModel(userIdParsed, isAdmin))!;
    }
}