using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Presentation.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddFastEndpoints();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        // Further configuration is only for Development environment
        if (!environment.IsDevelopment()) return services;

        services.SwaggerDocument(o =>
        {
            o.AutoTagPathSegmentIndex = 0;
            o.EnableJWTBearerAuth = false;
            
            o.DocumentSettings = s =>
            {
                s.Title = "Expense Tracker API";
                s.Version = "V1";
            };
        });
        
        return services;
    }
    
    public static WebApplication UsePresentationWithFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints();

        // Further configuration is only for Development environment
        if (!app.Environment.IsDevelopment()) return app;

        app.UseSwaggerGen();

        return app;
    }
}