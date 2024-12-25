using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChanges(CancellationToken cancellationToken = default) => await dbContext.SaveChangesAsync(cancellationToken);

    private readonly Lazy<IUserRepository> _lazyUserRepository = new(() => new UserRepository(dbContext));
    public IUserRepository BuildUserRepository() => _lazyUserRepository.Value;
    
    private readonly Lazy<IWorkspaceRepository> _lazyWorkspaceRepository = new(() => new WorkspaceRepository(dbContext));
    public IWorkspaceRepository BuildWorkspaceRepository() => _lazyWorkspaceRepository.Value;
    
    private readonly Lazy<ICurrencyRepository> _lazyCurrencyRepository = new(() => new CurrencyRepository(dbContext));
    public ICurrencyRepository BuildCurrencyRepository() => _lazyCurrencyRepository.Value;
    
    private readonly Lazy<IBalanceRepository> _lazyBalanceRepository = new(() => new BalanceRepository(dbContext));
    public IBalanceRepository BuildBalanceRepository() => _lazyBalanceRepository.Value;
    
    private readonly Lazy<ITransferTransactionRepository> _lazyTransferTransactionRepository = new(() => new TransferTransactionRepository(dbContext));
    public ITransferTransactionRepository BuildTransferTransactionRepository() => _lazyTransferTransactionRepository.Value;
}