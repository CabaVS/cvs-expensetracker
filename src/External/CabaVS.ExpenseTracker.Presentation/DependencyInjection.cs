using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using CabaVS.ExpenseTracker.Presentation.Middleware;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CabaVS.ExpenseTracker.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = bool.Parse(
                    builder.Configuration["Authentication:RequireHttpsMetadata"]
                    ?? throw new InvalidOperationException("RequireHttpsMetadata is not configured."));
                o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]
                    ?? throw new InvalidOperationException("MetadataAddress is not configured.");
                
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
        services.AddAuthorization();
        
        services.AddHttpContextAccessor();

        services.AddScoped<UserIdEnrichmentMiddleware>();
        
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        services.AddFastEndpoints();

        if (!builder.Environment.IsDevelopment())
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

        app.UseMiddleware<UserIdEnrichmentMiddleware>();
        
        app.UseFastEndpoints();

        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwaggerGen();

        return app;
    }
}
