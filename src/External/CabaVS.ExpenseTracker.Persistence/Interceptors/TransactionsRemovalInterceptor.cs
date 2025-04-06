using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CabaVS.ExpenseTracker.Persistence.Interceptors;

internal sealed class TransactionsRemovalInterceptor : ISaveChangesInterceptor
{
    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not ApplicationDbContext context)
        {
            throw new InvalidOperationException("Expected to have DbContext to work with.");
        }
        
        Guid[] removedBalances = context.ChangeTracker
            .Entries<BalanceEf>()
            .Where(x => x.State == EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .Distinct()
            .ToArray();
        Guid[] removedCategories = context.ChangeTracker
            .Entries<CategoryEf>()
            .Where(x => x.State == EntityState.Deleted)
            .Select(x => x.Entity.Id)
            .Distinct()
            .ToArray();
        
        if (removedBalances.Length > 0)
        {
            TransactionEf[] transactionsToRemove = await context.Transactions
                .Where(t => 
                    t.SourceBalanceId.HasValue && removedBalances.Contains(t.SourceBalanceId.Value) || 
                    t.DestinationBalanceId.HasValue && removedBalances.Contains(t.DestinationBalanceId.Value))
                .ToArrayAsync(cancellationToken);
            
            context.Transactions.RemoveRange(transactionsToRemove);
        }
        
        if (removedCategories.Length > 0)
        {
            TransactionEf[] transactionsToRemove = await context.Transactions
                .Where(t => 
                    t.SourceCategoryId.HasValue && removedBalances.Contains(t.SourceCategoryId.Value) || 
                    t.DestinationCategoryId.HasValue && removedBalances.Contains(t.DestinationCategoryId.Value))
                .ToArrayAsync(cancellationToken);
            
            context.Transactions.RemoveRange(transactionsToRemove);
        }
        
        return result;
    }
}
