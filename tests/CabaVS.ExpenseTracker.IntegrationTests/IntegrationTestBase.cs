using CabaVS.ExpenseTracker.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.IntegrationTests;

[TestCaseOrderer("CabaVS.ExpenseTracker.IntegrationTests.TestCaseOrderer", "CabaVS.ExpenseTracker.IntegrationTests")]
public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    
    protected readonly HttpClient Client;
    
    // Limitations of using internal classes inside xUnit public tests
    protected static T ConvertTo<T>(object obj) => (T)obj;
    protected readonly object DbContext;
    
    protected IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        _scope = factory.Services.CreateScope();
        
        Client = factory.CreateClient();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            ConvertTo<ApplicationDbContext>(DbContext).Dispose();
            Client.Dispose();
            _scope.Dispose();
        }
    }
}
