using CabaVS.ExpenseTracker.Web.Configuration.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
    .AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(
        OpenIdConnectDefaults.AuthenticationScheme,
        options =>
        {
            var configModel = configuration.GetSection("IdentityServer").Get<IdentityServerConfigurationModel>() 
                              ?? throw new InvalidOperationException("Identity Server section not found in configuration.");
            
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.CallbackPath = new PathString("/signin-oidc");
            options.SignedOutCallbackPath = new PathString("/signout-callback-oidc");

            options.Authority = configModel.Authority ?? throw new InvalidOperationException("Authority not configured.");
            options.ResponseType = OpenIdConnectResponseType.Code;
            
            options.ClientId = configModel.ClientId ?? throw new InvalidOperationException("Client Id not configured.");
            options.ClientSecret = configModel.ClientSecret ?? throw new InvalidOperationException("Client Secret not configured.");

            options.DisableTelemetry = true;
            options.SaveTokens = true;
            options.MapInboundClaims = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            
            options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            options.TokenValidationParameters.RoleClaimType = "role";
            
            options.Scope.Add(configModel.Scope ?? throw new InvalidOperationException("Scope not configured."));
        });

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
