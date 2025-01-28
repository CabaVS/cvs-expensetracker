using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal class GenericCommandRepository<TDomain, TPersistence> : IGenericCommandRepository<TDomain> 
    where TDomain : AuditableEntity
    where TPersistence : class, IRepresentAuditableEntity<TDomain, TPersistence>, new()
{
    protected readonly ApplicationDbContext DbContext;
    
    public GenericCommandRepository(ApplicationDbContext dbContext) => DbContext = dbContext;

    public virtual async Task<TDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TPersistence? persistenceEntity = await DbContext.Set<TPersistence>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return persistenceEntity?.ToDomainEntity();
    }

    public virtual async Task<Guid> CreateAsync(TDomain entity, CancellationToken cancellationToken = default)
    {
        TPersistence persistenceEntity = new TPersistence().FromDomainEntity(entity);
        
        _ = await DbContext.Set<TPersistence>().AddAsync(persistenceEntity, cancellationToken);

        return entity.Id;
    }

    public virtual Task<bool> UpdateAsync(TDomain entity, CancellationToken cancellationToken = default)
    {
        TPersistence persistenceEntity = new TPersistence().FromDomainEntity(entity);
        
        _ = DbContext.Set<TPersistence>().Update(persistenceEntity);
        
        return Task.FromResult(true);
    }

    public virtual Task<bool> DeleteAsync(TDomain entity, CancellationToken cancellationToken = default)
    {
        TPersistence persistenceEntity = new TPersistence().FromDomainEntity(entity);
        
        _ = DbContext.Set<TPersistence>().Remove(persistenceEntity);
        
        return Task.FromResult(true);
    }
}
