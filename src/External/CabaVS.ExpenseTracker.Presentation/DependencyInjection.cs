using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Presentation.Endpoints;
using CabaVS.ExpenseTracker.Presentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection serviceCollection, IWebHostEnvironment environment)
    {
        serviceCollection.RegisterAllEndpoints();
        
        serviceCollection.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        if (!environment.IsDevelopment()) return serviceCollection;
        
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Expense Tracker API"
                    });
            });
        
        return serviceCollection;
    }
    
    public static WebApplication ConfigureSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;
        
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static WebApplication MapAllEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }

    private static void RegisterAllEndpoints(this IServiceCollection serviceCollection)
    {
        var serviceDescriptors = AssemblyMarker.Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));
        
        serviceCollection.TryAddEnumerable(serviceDescriptors);
    }
}