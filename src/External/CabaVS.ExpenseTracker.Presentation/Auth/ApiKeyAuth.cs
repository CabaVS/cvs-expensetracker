using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CabaVS.ExpenseTracker.Presentation.Auth;

internal sealed class ApiKeyAuth(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration config)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    internal const string SchemeName = "ApiKey";
    internal const string HeaderName = "x-api-key";

    private readonly string _apiKey = config["Auth:ApiKey"] ?? throw new InvalidOperationException("Api key not set.");

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (IsPublicEndpoint())
            return Task.FromResult(AuthenticateResult.NoResult());

        if (!Request.Headers.TryGetValue(HeaderName, out var extractedApiKey) || !extractedApiKey.Equals(_apiKey))
            return Task.FromResult(AuthenticateResult.Fail("Invalid API credentials!"));

        var identity = new ClaimsIdentity(
            claims: new[]
            {
                new Claim("UserId", new Guid("59327D2E-2829-43CC-9A9F-4D2B1BDC271A").ToString()),
                new Claim("FullName", "Test User"),
                new Claim("IsAdmin", true.ToString())
            },
            authenticationType: Scheme.Name);
        var principal = new GenericPrincipal(identity, roles: null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private bool IsPublicEndpoint()
        => Context.GetEndpoint()?.Metadata.OfType<AllowAnonymousAttribute>().Any() is null or true;
}