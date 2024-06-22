using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Interceptors;

internal sealed class TransactionsRemovalInterceptor : ISaveChangesInterceptor
{
    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not ApplicationDbContext context)
            throw new InvalidOperationException("Expected to have DbContext to work with.");

        var expenseTransactionsToRemove = new List<ExpenseTransaction>();
        var incomeTransactionsToRemove = new List<IncomeTransaction>();
        var transferTransactionsToRemove = new List<TransferTransaction>();

        var workspacesToRemove = context.ChangeTracker.Entries<Workspace>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity.Id)
            .Distinct()
            .ToArray();
        if (workspacesToRemove.Length > 0)
        {
            var expenseTransactions = await context.ExpenseTransactions
                .Where(et =>
                    workspacesToRemove.Contains(et.Source.WorkspaceId) ||
                    workspacesToRemove.Contains(et.Destination.WorkspaceId))
                .Select(et => new ExpenseTransaction { Id = et.Id })
                .ToArrayAsync(cancellationToken);
            expenseTransactionsToRemove.AddRange(expenseTransactions);
            
            var incomeTransactions = await context.IncomeTransactions
                .Where(it =>
                    workspacesToRemove.Contains(it.Source.WorkspaceId) ||
                    workspacesToRemove.Contains(it.Destination.WorkspaceId))
                .Select(et => new IncomeTransaction { Id = et.Id })
                .ToArrayAsync(cancellationToken);
            incomeTransactionsToRemove.AddRange(incomeTransactions);
            
            var transferTransactions = await context.TransferTransactions
                .Where(tt =>
                    workspacesToRemove.Contains(tt.Source.WorkspaceId) ||
                    workspacesToRemove.Contains(tt.Destination.WorkspaceId))
                .Select(tt => new TransferTransaction { Id = tt.Id })
                .ToArrayAsync(cancellationToken);
            transferTransactionsToRemove.AddRange(transferTransactions);
        }
        
        var balancesToRemove = context.ChangeTracker.Entries<Balance>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity.Id)
            .Distinct()
            .ToArray();
        if (balancesToRemove.Length > 0)
        {
            var expenseTransactions = await context.ExpenseTransactions
                .Where(et => balancesToRemove.Contains(et.SourceId))
                .Select(et => new ExpenseTransaction { Id = et.Id })
                .ToArrayAsync(cancellationToken);
            expenseTransactionsToRemove.AddRange(expenseTransactions);
            
            var incomeTransactions = await context.IncomeTransactions
                .Where(it => balancesToRemove.Contains(it.DestinationId))
                .Select(et => new IncomeTransaction { Id = et.Id })
                .ToArrayAsync(cancellationToken);
            incomeTransactionsToRemove.AddRange(incomeTransactions);
            
            var transferTransactions = await context.TransferTransactions
                .Where(tt =>
                    balancesToRemove.Contains(tt.SourceId) ||
                    balancesToRemove.Contains(tt.DestinationId))
                .Select(tt => new TransferTransaction { Id = tt.Id })
                .ToArrayAsync(cancellationToken);
            transferTransactionsToRemove.AddRange(transferTransactions);
        }
        
        var expenseCategoriesToRemove = context.ChangeTracker.Entries<ExpenseCategory>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity.Id)
            .Distinct()
            .ToArray();
        if (expenseCategoriesToRemove.Length > 0)
        {
            var expenseTransactions = await context.ExpenseTransactions
                .Where(et => expenseCategoriesToRemove.Contains(et.DestinationId))
                .Select(et => new ExpenseTransaction { Id = et.Id })
                .ToArrayAsync(cancellationToken);
            expenseTransactionsToRemove.AddRange(expenseTransactions);
        }
        
        var incomeCategoriesToRemove = context.ChangeTracker.Entries<IncomeCategory>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity.Id)
            .Distinct()
            .ToArray();
        if (incomeCategoriesToRemove.Length > 0)
        {
            var incomeTransactions = await context.IncomeTransactions
                .Where(it => incomeCategoriesToRemove.Contains(it.DestinationId))
                .Select(et => new IncomeTransaction { Id = et.Id })
                .ToArrayAsync(cancellationToken);
            incomeTransactionsToRemove.AddRange(incomeTransactions);
        }

        context.ExpenseTransactions.RemoveRange(expenseTransactionsToRemove);
        context.IncomeTransactions.RemoveRange(incomeTransactionsToRemove);
        context.TransferTransactions.RemoveRange(transferTransactionsToRemove);

        return result;
    }
}