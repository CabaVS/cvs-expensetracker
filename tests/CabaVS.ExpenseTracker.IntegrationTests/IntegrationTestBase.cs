using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CabaVS.ExpenseTracker.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace CabaVS.ExpenseTracker.IntegrationTests;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    
    private readonly IServiceScope _scope;
    
    protected readonly HttpClient Client;
    
    // Limitations of using internal classes inside xUnit public tests
    protected static T ConvertTo<T>(object obj) => (T)obj;
    protected readonly object DbContext;
    
    protected IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        // Tests configuration, not an application itself
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationTestBase>()
            .AddEnvironmentVariables()
            .Build();
        
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {configuration.GetValue<string>("ApiToken")}");
        
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
    
    protected static StringContent ToJsonContent<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonSerializerOptions);
        return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    }
    
    public void Dispose()
    {
        ConvertTo<ApplicationDbContext>(DbContext).Dispose();
        Client.Dispose();
        
        _scope.Dispose();
        
        GC.SuppressFinalize(this);
    }
}