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
    public static IServiceCollection AddPresentation(this IServiceCollection serviceCollection, IWebHostEnvironment environment)
    {
        serviceCollection.AddFastEndpoints();
        
        serviceCollection.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        if (!environment.IsDevelopment()) return serviceCollection;

        serviceCollection.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "Expense Tracker API";
                s.Version = "v1";
            };

            o.AutoTagPathSegmentIndex = 0;
            o.EnableJWTBearerAuth = false;
        });
        
        return serviceCollection;
    }

    public static WebApplication UseFastEndpoints(this WebApplication app)
    {
        MainExtensions.UseFastEndpoints(app);

        if (!app.Environment.IsDevelopment()) return app;

        app.UseSwaggerGen();

        return app;
    }
}