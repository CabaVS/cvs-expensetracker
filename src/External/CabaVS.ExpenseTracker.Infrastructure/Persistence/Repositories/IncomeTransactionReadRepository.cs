using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeTransactionReadRepository(ApplicationDbContext dbContext) : IIncomeTransactionReadRepository
{
    public async Task<IncomeTransactionModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.IncomeTransactions
            .Include(et => et.Source)
            .Include(et => et.Destination)
            .Where(et => et.Source.WorkspaceId == workspaceId)
            .Where(et => et.Destination.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<IncomeTransactionModel?> GetById(Guid incomeTransactionId, Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.IncomeTransactions
            .Include(et => et.Source)
            .Include(et => et.Destination)
            .Where(et => et.Id == incomeTransactionId)
            .Where(et => et.Source.WorkspaceId == workspaceId)
            .Where(et => et.Destination.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }
    
    private static readonly Expression<Func<IncomeTransaction, IncomeTransactionModel>> EntityToModelProjection =
        incomeTransaction => new IncomeTransactionModel(
            incomeTransaction.Id,
            incomeTransaction.Date,
            new CategoryUnderTransactionModel(
                incomeTransaction.Destination.Id,
                incomeTransaction.Destination.Name),
            incomeTransaction.AmountInSourceCurrency,
            new BalanceUnderTransactionModel(
                incomeTransaction.Source.Id,
                incomeTransaction.Source.Name),
            incomeTransaction.AmountInDestinationCurrency,
            incomeTransaction.Tags);
}