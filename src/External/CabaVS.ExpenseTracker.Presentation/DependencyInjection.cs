using System.Text;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Presentation.Middleware;
using CabaVS.ExpenseTracker.Presentation.Services;
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
        services.AddFastEndpoints();

        services.AddHttpContextAccessor();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Authentication:JwtBearer:SigningKey"] ??
                            throw new InvalidOperationException("SigningKey is not configured."))),
                    ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
                    ValidAudience = configuration["Authentication:JwtBearer:Audience"],
                    
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
        services.AddAuthorization();

        services.AddScoped<UserCreationMiddleware>();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        
        // Further configuration is only for Development environment
        if (!environment.IsDevelopment()) return services;

        services.SwaggerDocument(o =>
        {
            o.AutoTagPathSegmentIndex = 0;
            o.EnableJWTBearerAuth = true;
            
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<UserCreationMiddleware>();

        // Further configuration is only for Development environment
        if (!app.Environment.IsDevelopment()) return app;

        app.UseSwaggerGen();

        return app;
    }
}