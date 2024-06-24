using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.IntegrationTests.Common;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
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
    private readonly MsSqlContainer _dbContainer =
        new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04")
            .Build();

    static IntegrationTestWebAppFactory()
    {
        SharedState.Instance.Add(StateKeys.AuthenticatedUser, new Guid("00000000-0000-0000-0000-000000000001"));
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(_dbContainer.GetConnectionString()),
                ServiceLifetime.Transient,
                ServiceLifetime.Transient);

            services.RemoveAll<ICurrentUserAccessor>();
            services.AddScoped<ICurrentUserAccessor>(
                _ => new CurrentUserAccessorInjected(
                    new AuthorizedUserModel(
                        SharedState.Instance[StateKeys.AuthenticatedUser],
                        "Test User",
                        true)));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}