using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChanges(CancellationToken cancellationToken = default) => await dbContext.SaveChangesAsync(cancellationToken);

    private readonly Lazy<IWorkspaceRepository> _lazyWorkspaceRepository = new(() => new WorkspaceRepository(dbContext));
    public IWorkspaceRepository BuildWorkspaceRepository() => _lazyWorkspaceRepository.Value;
}