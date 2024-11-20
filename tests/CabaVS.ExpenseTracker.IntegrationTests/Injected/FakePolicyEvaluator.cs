using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.IntegrationTests.Injected;

internal sealed class FakePolicyEvaluator : IPolicyEvaluator
{
    private const string SchemeName = "FakeScheme";
    
    public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var principal = new ClaimsPrincipal();

        principal.AddIdentity(
            new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, CurrentUserAccessorInjected.AuthenticatedUserId.ToString()),
            ], SchemeName));

        return await Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(principal, new AuthenticationProperties(), SchemeName)));
    }

    public async Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object? resource) => await Task.FromResult(PolicyAuthorizationResult.Success());
}