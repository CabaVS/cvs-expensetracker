using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CabaVS.ExpenseTracker.Persistence.Interceptors;

internal sealed class TransactionsRemovalInterceptor : ISaveChangesInterceptor
{
    private const int BatchSize = 1000;
    
    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not ApplicationDbContext context)
            throw new InvalidOperationException("Expected to have DbContext to work with.");

        var removedBalances = context.ChangeTracker
            .Entries<Balance>()
            .Where(x => x.State == EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .Distinct()
            .ToArray();
        if (removedBalances.Length > 0)
        {
            for (var i = 0; ; i++)
            {
                var transferTransactionsToRemove = await context.TransferTransactions
                    .Where(x => removedBalances.Contains(x.SourceId) ||
                                                removedBalances.Contains(x.DestinationId))
                    .Skip(i * BatchSize)
                    .Take(BatchSize)
                    .OrderBy(x => x.Id)
                    .ToArrayAsync(cancellationToken);
                context.TransferTransactions.RemoveRange(transferTransactionsToRemove);
                
                if (transferTransactionsToRemove.Length < BatchSize)
                    break;
            }
        }
        
        return result;
    }
}