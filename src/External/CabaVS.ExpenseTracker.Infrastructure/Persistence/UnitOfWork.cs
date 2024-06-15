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
    
    private readonly Lazy<IBalanceWriteRepository> _lazyBalanceWriteRepository = new(() => new BalanceWriteRepository(dbContext));
    public IBalanceWriteRepository BalanceWriteRepository => _lazyBalanceWriteRepository.Value;
    
    private readonly Lazy<IIncomeCategoryWriteRepository> _lazyIncomeCategoryWriteRepository = new(() => new IncomeCategoryWriteRepository(dbContext));
    public IIncomeCategoryWriteRepository IncomeCategoryWriteRepository => _lazyIncomeCategoryWriteRepository.Value;
    
    public async Task SaveChanges(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);
}