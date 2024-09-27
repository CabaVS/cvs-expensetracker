using System.Security.Claims;
using CabaVS.ExpenseTracker.Presentation.Auth;
using CabaVS.ExpenseTracker.Presentation.Configuration.Models;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddFastEndpoints();
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => options.ApplyFromConfig(configuration));

        services
            .AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.User, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "User"); // TODO: better use a separate NuGet for this
            })
            .AddPolicy(PolicyNames.Admin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Admin"); // TODO: better use a separate NuGet for this
            });
        
        if (!environment.IsDevelopment()) return services;

        services.SwaggerDocument(options =>
        {
            options.AutoTagPathSegmentIndex = 0;

            options.DocumentSettings = settings =>
            {
                settings.Title = "[CVS] Expense Tracker API";
                settings.Version = "v1";
            };
        });

        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints();
        
        if (!app.Environment.IsDevelopment()) return app;

        app.UseSwaggerGen();
        
        return app;
    }
}