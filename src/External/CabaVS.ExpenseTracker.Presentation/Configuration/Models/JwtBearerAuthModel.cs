using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CabaVS.ExpenseTracker.Presentation.Configuration.Models;

internal sealed class JwtBearerAuthModel
{
    public string Authority { get; set; } = default!;
    public string Audience { get; set; } = default!;
}

internal static partial class ConfigurationExtensions
{
    public static void ApplyFromConfig(
        this JwtBearerOptions jwtBearerOptions,
        IConfiguration configuration,
        string configKey = "JwtBearerAuth")
    {
        var configModel = configuration.GetSection(configKey).Get<JwtBearerAuthModel>() 
                          ?? throw new InvalidOperationException("JWT Bearer Auth configuration not found.");
        
        if (string.IsNullOrWhiteSpace(configModel.Authority))
            throw new InvalidOperationException("JWT Bearer authority configuration required.");
        jwtBearerOptions.Authority = configModel.Authority;

        var tokenValidationParameters = new TokenValidationParameters();
        
        if (!string.IsNullOrWhiteSpace(configModel.Audience))
        {
            jwtBearerOptions.Audience = configModel.Audience;
            tokenValidationParameters.ValidateAudience = true;
        }
        
        jwtBearerOptions.TokenValidationParameters = tokenValidationParameters;
    }
}