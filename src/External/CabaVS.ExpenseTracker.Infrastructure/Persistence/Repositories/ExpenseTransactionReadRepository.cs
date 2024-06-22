using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExpenseTransactionReadRepository(ApplicationDbContext dbContext) : IExpenseTransactionReadRepository
{
    public async Task<ExpenseTransactionModel[]> GetAll(Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.ExpenseTransactions
            .Include(et => et.Source)
            .Include(et => et.Destination)
            .Where(et => et.Source.WorkspaceId == workspaceId)
            .Where(et => et.Destination.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .ToArrayAsync(ct);
    }

    public async Task<ExpenseTransactionModel?> GetById(Guid expenseTransactionId, Guid workspaceId, CancellationToken ct = default)
    {
        return await dbContext.ExpenseTransactions
            .Include(et => et.Source)
            .Include(et => et.Destination)
            .Where(et => et.Id == expenseTransactionId)
            .Where(et => et.Source.WorkspaceId == workspaceId)
            .Where(et => et.Destination.WorkspaceId == workspaceId)
            .Select(EntityToModelProjection)
            .FirstOrDefaultAsync(ct);
    }
    
    private static readonly Expression<Func<ExpenseTransaction, ExpenseTransactionModel>> EntityToModelProjection =
        expenseTransaction => new ExpenseTransactionModel(
            expenseTransaction.Id,
            expenseTransaction.Date,
            new BalanceUnderTransactionModel(
                expenseTransaction.Source.Id,
                expenseTransaction.Source.Name),
            expenseTransaction.AmountInSourceCurrency,
            new CategoryUnderTransactionModel(
                expenseTransaction.Destination.Id,
                expenseTransaction.Destination.Name),
            expenseTransaction.AmountInDestinationCurrency,
            expenseTransaction.Tags);
}