using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class BalanceReadRepository(ApplicationDbContext dbContext) : IBalanceReadRepository
{
    public async Task<BalanceModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.Balances
            .Include(b => b.Currency)
            .Where(b => b.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<BalanceModel?> GetById(Guid workspaceId, Guid balanceId, CancellationToken ct = default)
    {
        return await dbContext.Balances
            .Include(b => b.Currency)
            .Where(b => b.WorkspaceId == workspaceId)
            .Where(b => b.Id == balanceId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }

    private static readonly Expression<Func<Balance, BalanceModel>> EntityToModelProjection =
        balance => new BalanceModel(
            balance.Id, 
            balance.Name, 
            balance.Amount,
            new CurrencyModel(
                balance.Currency.Id,
                balance.Currency.Name,
                balance.Currency.Code,
                balance.Currency.Symbol));
}