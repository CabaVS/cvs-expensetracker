using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CategoryCommandRepository(ApplicationDbContext dbContext)
    : GenericCommandRepository<Domain.Entities.Category, Category>(dbContext)
{
    public override async Task<Domain.Entities.Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Category? category = await DbContext.Categories
            .AsNoTracking()
            .Include(c => c.Currency)
            .Include(c => c.Workspace)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return category?.ToDomainEntity();
    }
}
