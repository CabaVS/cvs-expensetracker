using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransactionCommandRepository(ApplicationDbContext dbContext)
    : GenericCommandRepository<Domain.Entities.Transaction, Transaction>(dbContext)
{
    public override async Task<Domain.Entities.Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Transaction? transaction = await DbContext.Transactions
            .AsNoTracking()
            .Include(t => t.Workspace)
            .Include(t => t.SourceBalance).ThenInclude(b => b!.Currency)
            .Include(t => t.SourceBalance).ThenInclude(b => b!.Workspace)
            .Include(t => t.DestinationBalance).ThenInclude(b => b!.Currency)
            .Include(t => t.DestinationBalance).ThenInclude(b => b!.Workspace)
            .Include(t => t.SourceCategory).ThenInclude(c => c!.Currency)
            .Include(t => t.SourceCategory).ThenInclude(c => c!.Workspace)
            .Include(t => t.DestinationCategory).ThenInclude(c => c!.Currency)
            .Include(t => t.DestinationCategory).ThenInclude(c => c!.Workspace)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return transaction?.ToDomainEntity();
    }
}
