using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;

public interface IRepresentAuditableEntity<TDomain, out TPersistence> : IAuditableEntity
    where TDomain : AuditableEntity
    where TPersistence : class
{
    TDomain ToDomainEntity();
    TPersistence FromDomainEntity(TDomain domainEntity);
}
