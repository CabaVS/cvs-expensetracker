using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        IEnumerable<EntityEntry<IAuditableEntity>> createdEntities = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(x => x.State == EntityState.Added);
        foreach (EntityEntry<IAuditableEntity> createdEntity in createdEntities)
        {
            createdEntity.Entity.CreatedOn = utcNow;
        }

        IEnumerable<EntityEntry<IAuditableEntity>> modifiedEntities = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(x => x.State == EntityState.Modified);
        foreach (EntityEntry<IAuditableEntity> modifiedEntity in modifiedEntities)
        {
            modifiedEntity.Entity.ModifiedOn = utcNow;
        }
        
        return ValueTask.FromResult(result);
    }
}
