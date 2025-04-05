using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CurrencyRepository(ApplicationDbContext dbContext) : ICurrencyRepository
{
    public async Task<Currency?> GetByIdAsync(Guid currencyId, CancellationToken cancellationToken = default)
    {
        Currency? currency = await dbContext.Currencies
            .Where(c => c.Id == currencyId)
            .Select(c => c.ToDomain())
            .FirstOrDefaultAsync(cancellationToken);
        return currency;
    }
}
