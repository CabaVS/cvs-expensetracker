using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment environment)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = bool.Parse(
                    configuration["Authentication:RequireHttpsMetadata"]
                    ?? throw new InvalidOperationException("RequireHttpsMetadata is not configured."));
                o.MetadataAddress = configuration["Authentication:MetadataAddress"]
                    ?? throw new InvalidOperationException("MetadataAddress is not configured.");
                
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidAudience = configuration["Authentication:Audience"],
                    
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
        services.AddAuthorization();
        
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        services.AddFastEndpoints();

        if (!environment.IsDevelopment())
        {
            return services;
        }
        
        services.SwaggerDocument(x => 
        {
            x.AutoTagPathSegmentIndex = 0;
            x.EnableJWTBearerAuth = true;
            
            x.DocumentSettings = s =>
            {
                s.Title = "Expense Tracker API";
                s.Version = "v1";
            }; 
        });
        
        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseFastEndpoints();

        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwaggerGen();

        return app;
    }
}
