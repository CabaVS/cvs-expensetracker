using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceCommandRepository(ApplicationDbContext dbContext)
    : GenericCommandRepository<Domain.Entities.Balance, Balance>(dbContext)
{
    public override async Task<Domain.Entities.Balance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Balance? category = await DbContext.Balances
            .AsNoTracking()
            .Include(b => b.Currency)
            .Include(b => b.Workspace)
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return category?.ToDomainEntity();
    }
}
