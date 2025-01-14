using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceReadRepository(ApplicationDbContext dbContext) : IBalanceReadRepository
{
    public async Task<BalanceModel[]> GetBalances(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        BalanceModel[] balances = await dbContext.Balances
            .Where(b => b.WorkspaceId == workspaceId)
            .Select(b => new BalanceModel(
                b.Id, b.Name, b.Amount, 
                new CurrencyModel(
                    b.Currency.Id, b.Currency.Name, b.Currency.Code, b.Currency.Symbol)))
            .ToArrayAsync(cancellationToken);
        return balances;
    }

    public async Task<BalanceModel?> GetBalanceById(Guid balanceId, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        BalanceModel? balance = await dbContext.Balances
            .Where(b => b.Id == balanceId)
            .Where(b => b.WorkspaceId == workspaceId)
            .Select(b => new BalanceModel(
                b.Id, b.Name, b.Amount, 
                new CurrencyModel(
                    b.Currency.Id, b.Currency.Name, b.Currency.Code, b.Currency.Symbol)))
            .FirstOrDefaultAsync(cancellationToken);
        return balance;
    }
}
