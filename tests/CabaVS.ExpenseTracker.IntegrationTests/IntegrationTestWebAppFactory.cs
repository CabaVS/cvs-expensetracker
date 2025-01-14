using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
using CabaVS.ExpenseTracker.IntegrationTests.Seeders;
using CabaVS.ExpenseTracker.Persistence;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace CabaVS.ExpenseTracker.IntegrationTests;

public sealed class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
#pragma warning disable CA2213
    private readonly MsSqlContainer _dbContainer =
#pragma warning restore CA2213
        new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU16-ubuntu-22.04")
            .Build();
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using IServiceScope scope = Services.CreateScope();
        
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        
        DefaultDatabaseSeeder.Seed(dbContext);
    }

    public new async Task DisposeAsync() => await _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(_dbContainer.GetConnectionString()),
                ServiceLifetime.Transient,
                ServiceLifetime.Transient);

            services.RemoveAll<ICurrentUserAccessor>();
            services.AddScoped<ICurrentUserAccessor>(_ => new CurrentUserAccessorInjected());
            
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
        });
}
