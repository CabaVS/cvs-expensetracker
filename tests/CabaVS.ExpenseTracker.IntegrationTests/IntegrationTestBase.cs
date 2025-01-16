using CabaVS.ExpenseTracker.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CabaVS.ExpenseTracker.IntegrationTests;

[TestCaseOrderer("CabaVS.ExpenseTracker.IntegrationTests.TestCaseOrderer", "CabaVS.ExpenseTracker.IntegrationTests")]
public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    
    internal HttpClient Client { get; }
    internal ApplicationDbContext DbContext { get; }
    
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
            DbContext.Dispose();
            Client.Dispose();
            
            _scope.Dispose();
        }
    }
}
