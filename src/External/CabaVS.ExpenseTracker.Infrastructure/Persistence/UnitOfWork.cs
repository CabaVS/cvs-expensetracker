using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly Lazy<ICurrencyWriteRepository> _lazyCurrencyWriteRepository = new(() => new CurrencyWriteRepository(dbContext));
    public ICurrencyWriteRepository CurrencyWriteRepository => _lazyCurrencyWriteRepository.Value;
    
    private readonly Lazy<IWorkspaceWriteRepository> _lazyWorkspaceWriteRepository = new(() => new WorkspaceWriteRepository(dbContext));
    public IWorkspaceWriteRepository WorkspaceWriteRepository => _lazyWorkspaceWriteRepository.Value;
    
    public async Task SaveChanges(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);
}