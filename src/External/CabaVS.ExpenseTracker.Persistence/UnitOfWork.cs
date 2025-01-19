using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChanges(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);

    private readonly Lazy<IUserRepository> _userRepository = new(() => new UserRepository(dbContext));
    public IUserRepository UserRepository => _userRepository.Value;
    
    private readonly Lazy<IWorkspaceRepository> _workspaceRepository = new(() => new WorkspaceRepository(dbContext));
    public IWorkspaceRepository WorkspaceRepository => _workspaceRepository.Value;
    
    private readonly Lazy<ICurrencyRepository> _currencyRepository = new(() => new CurrencyRepository(dbContext));
    public ICurrencyRepository CurrencyRepository => _currencyRepository.Value;
    
    private readonly Lazy<IBalanceRepository> _balanceRepository = new(() => new BalanceRepository(dbContext));
    public IBalanceRepository BalanceRepository => _balanceRepository.Value;
    
    private readonly Lazy<IExpenseCategoryRepository> _expenseCategoryRepository = new(() => new ExpenseCategoryRepository(dbContext));
    public IExpenseCategoryRepository ExpenseCategoryRepository => _expenseCategoryRepository.Value;

    private readonly Lazy<ITransferTransactionRepository> _transferTransactionRepository = new(() => new TransferTransactionRepository(dbContext));
    public ITransferTransactionRepository TransferTransactionRepository => _transferTransactionRepository.Value;
}
