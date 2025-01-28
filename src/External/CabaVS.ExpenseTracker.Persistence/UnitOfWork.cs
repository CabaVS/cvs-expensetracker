using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);

    private readonly Lazy<IGenericCommandRepository<User>> _userRepository = 
        new(() => new GenericCommandRepository<User, Persistence.Entities.User>(dbContext));
    public IGenericCommandRepository<User> UserRepository => _userRepository.Value;
    
    private readonly Lazy<IWorkspaceCommandRepository> _workspaceRepository = 
        new(() => new WorkspaceCommandRepository(dbContext));
    public IWorkspaceCommandRepository WorkspaceRepository => _workspaceRepository.Value;
    
    private readonly Lazy<IGenericCommandRepository<Currency>> _currencyRepository = 
        new(() => new GenericCommandRepository<Currency, Persistence.Entities.Currency>(dbContext));
    public IGenericCommandRepository<Currency> CurrencyRepository => _currencyRepository.Value;
    
    private readonly Lazy<IGenericCommandRepository<Balance>> _balanceRepository = 
        new(() => new GenericCommandRepository<Balance, Persistence.Entities.Balance>(dbContext));
    public IGenericCommandRepository<Balance> BalanceRepository => _balanceRepository.Value;
    
    private readonly Lazy<IGenericCommandRepository<Category>> _categoryRepository = 
        new(() => new GenericCommandRepository<Category, Persistence.Entities.Category>(dbContext));
    public IGenericCommandRepository<Category> CategoryRepository => _categoryRepository.Value;

    private readonly Lazy<IGenericCommandRepository<Transaction>> _transactionRepository = 
        new(() => new GenericCommandRepository<Transaction, Persistence.Entities.Transaction>(dbContext));
    public IGenericCommandRepository<Transaction> TransactionRepository => _transactionRepository.Value;
}
