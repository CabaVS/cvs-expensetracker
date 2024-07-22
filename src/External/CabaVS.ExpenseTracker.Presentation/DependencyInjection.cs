using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Presentation.Auth;
using CabaVS.ExpenseTracker.Presentation.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection serviceCollection, IWebHostEnvironment environment)
    {
        serviceCollection
            .AddAuthentication(ApiKeyAuth.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuth>(ApiKeyAuth.SchemeName, null);
        serviceCollection.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", x => x.RequireClaim("IsAdmin", true.ToString()));
        
        serviceCollection.AddFastEndpoints();

        serviceCollection.AddHttpContextAccessor();
        
        serviceCollection.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        if (!environment.IsDevelopment()) return serviceCollection;

        serviceCollection.SwaggerDocument(o =>
        {
            o.AutoTagPathSegmentIndex = 0;
            o.EnableJWTBearerAuth = false;
            
            o.DocumentSettings = s =>
            {
                s.Title = "Expense Tracker API";
                s.Version = "v1";
                
                s.AddAuth(ApiKeyAuth.SchemeName, new OpenApiSecurityScheme
                {
                    Name = ApiKeyAuth.HeaderName,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.ApiKey,
                });
            };
        });
        
        return serviceCollection;
    }

    public static WebApplication UseFastEndpoints(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        MainExtensions.UseFastEndpoints(app);

        if (!app.Environment.IsDevelopment()) return app;

        app.UseSwaggerGen();

        return app;
    }
}