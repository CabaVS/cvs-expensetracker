using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CurrencyRepository(ApplicationDbContext dbContext) : ICurrencyRepository
{
    public async Task<Domain.Entities.Currency?> GetById(Guid id, CancellationToken cancellationToken)
    {
        Currency? currency = await dbContext.Currencies
            .AsNoTracking()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return currency?.ConvertToDomain();
    }
}
