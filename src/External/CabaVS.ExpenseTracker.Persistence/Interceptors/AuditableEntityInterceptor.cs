using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CabaVS.ExpenseTracker.Persistence.Interceptors;

internal sealed class AuditableEntityInterceptor : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not ApplicationDbContext context)
        {
            throw new InvalidOperationException("Expected to have DbContext to work with.");
        }
        
        DateTime utcNow = DateTime.UtcNow;

        IEnumerable<IAuditableEntity> createdEntities = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(x => x.State is EntityState.Added)
            .Select(x => x.Entity);
        foreach (IAuditableEntity createdEntity in createdEntities)
        {
            createdEntity.CreatedOn = utcNow;
        }

        IEnumerable<IAuditableEntity> modifiedEntities = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified)
            .Select(x => x.Entity);
        foreach (IAuditableEntity modifiedEntity in modifiedEntities)
        {
            modifiedEntity.ModifiedOn = utcNow;
        }
        
        return ValueTask.FromResult(result);
    }
}
