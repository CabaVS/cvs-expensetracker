using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly Lazy<ICurrencyRepository> _lazyCurrencyRepository = new(() => new CurrencyRepository(dbContext));
    public ICurrencyRepository CurrencyRepository => _lazyCurrencyRepository.Value;
    
    private readonly Lazy<IWorkspaceRepository> _lazyWorkspaceRepository = new(() => new WorkspaceRepository(dbContext));
    public IWorkspaceRepository WorkspaceRepository => _lazyWorkspaceRepository.Value;
    
    private readonly Lazy<IBalanceRepository> _lazyBalanceRepository = new(() => new BalanceRepository(dbContext));
    public IBalanceRepository BalanceRepository => _lazyBalanceRepository.Value;
    
    private readonly Lazy<IExpenseCategoryRepository> _lazyExpenseCategoryRepository = new(() => new ExpenseCategoryRepository(dbContext));
    public IExpenseCategoryRepository ExpenseCategoryRepository => _lazyExpenseCategoryRepository.Value;
    
    private readonly Lazy<IIncomeCategoryRepository> _lazyIncomeCategoryRepository = new(() => new IncomeCategoryRepository(dbContext));
    public IIncomeCategoryRepository IncomeCategoryRepository => _lazyIncomeCategoryRepository.Value;
    
    private readonly Lazy<IIncomeTransactionRepository> _lazyIncomeTransactionRepository = new(() => new IncomeTransactionRepository(dbContext));
    public IIncomeTransactionRepository IncomeTransactionRepository => _lazyIncomeTransactionRepository.Value;
    
    public async Task SaveChanges(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);
}