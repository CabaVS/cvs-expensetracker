using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
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

        if (!environment.IsDevelopment())
        {
            return services;
        }
        
        services.SwaggerDocument(x => 
        {
            x.AutoTagPathSegmentIndex = 0;
            x.EnableJWTBearerAuth = false;
            
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
        
        app.UseFastEndpoints();

        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwaggerGen();

        return app;
    }
}
