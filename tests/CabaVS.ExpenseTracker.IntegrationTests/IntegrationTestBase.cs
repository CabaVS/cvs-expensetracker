using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.IntegrationTests.DatabaseSeeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace CabaVS.ExpenseTracker.IntegrationTests;

[TestCaseOrderer(
    ordererTypeName: "CabaVS.ExpenseTracker.IntegrationTests.Common.TestCaseOrderer",
    ordererAssemblyName: "CabaVS.ExpenseTracker.IntegrationTests")]
public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    private static readonly object Lock = new();
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private static bool _dbSeeded;
    
    private readonly IServiceScope _scope;
    
    protected readonly HttpClient Client;
    
    // Limitations of using internal classes inside xUnit public tests
    protected static T ConvertTo<T>(object obj) => (T)obj;
    protected readonly object DbContext;
    
    protected IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        Client = factory.CreateClient();
        
        _scope = factory.Services.CreateScope();
        var dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!_dbSeeded)
        {
            lock (Lock)
            {
                if (!_dbSeeded)
                {
                    dbContext.Database.Migrate();
                    DefaultSeeder.Seed(dbContext);
                    _dbSeeded = true;
                }
            }
        }

        DbContext = dbContext;
    }

    protected static StringContent ToJsonContent<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonSerializerOptions);
        return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }
}