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

        IEnumerable<EntityEntry<IAuditableEntity>> createdEntities = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(x => x.State == EntityState.Added)
            .Where(x => x.Entity.CreatedOn == default)
            .DistinctBy(x => x.Entity.Id);
        foreach (EntityEntry<IAuditableEntity> entity in createdEntities)
        {
            entity.Entity.CreatedOn = DateTime.UtcNow;
        }
        
        IEnumerable<EntityEntry<IAuditableEntity>> updatedEntities = context.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(x => x.State == EntityState.Modified)
            .DistinctBy(x => x.Entity.Id);
        foreach (EntityEntry<IAuditableEntity> entity in updatedEntities)
        {
            entity.Entity.ModifiedOn = DateTime.UtcNow;
        }

        return ValueTask.FromResult(result);
    }
}
