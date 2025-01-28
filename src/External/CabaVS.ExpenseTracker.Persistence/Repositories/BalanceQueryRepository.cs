using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class BalanceQueryRepository(ApplicationDbContext dbContext) : IBalanceQueryRepository
{
    public async Task<BalanceModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        BalanceModel[] allModels = await dbContext.Balances
            .Where(x => x.WorkspaceId == workspaceId)
            .Select(ProjectToModel)
            .ToArrayAsync(cancellationToken);
        return allModels;
    }

    public async Task<BalanceModel?> GetByIdAsync(Guid workspaceId, Guid balanceId, CancellationToken cancellationToken = default)
    {
        BalanceModel? model = await dbContext.Balances
            .Where(x => x.WorkspaceId == workspaceId)
            .Where(x => x.Id == balanceId)
            .Select(ProjectToModel)
            .FirstOrDefaultAsync(cancellationToken);
        return model;
    }

    private static Expression<Func<Balance, BalanceModel>> ProjectToModel =>
        x => new BalanceModel(
            x.Id, x.Name, x.Amount, new CurrencyModel(
                x.Currency.Id, x.Currency.Name, x.Currency.Code, x.Currency.Symbol));
}
