using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct = default);
    
    ICurrencyWriteRepository CurrencyWriteRepository { get; }
    IWorkspaceWriteRepository WorkspaceWriteRepository { get; }
    IBalanceWriteRepository BalanceWriteRepository { get; }
    IIncomeCategoryWriteRepository IncomeCategoryWriteRepository { get; }
}