using CabaVS.ExpenseTracker.Domain.Abstractions;

namespace CabaVS.ExpenseTracker.Domain.Primitives;

public abstract class AuditableEntity(Guid id, DateTime createdOn, DateTime modifiedOn)
    : IAuditableEntity, IEquatable<AuditableEntity>
{
    public Guid Id { get; } = id;
    public DateTime CreatedOn { get; } = createdOn;
    public DateTime ModifiedOn { get; } = modifiedOn;

    public static bool operator ==(AuditableEntity? first, AuditableEntity? second) =>
        first is not null &&
        first.Equals(second);
    
    public static bool operator !=(AuditableEntity? first, AuditableEntity? second) =>
        !(first == second);
    
    public bool Equals(AuditableEntity? other) =>
        other is not null && 
        other.GetType() == GetType() &&
        other.Id == Id;

    public override bool Equals(object? obj) =>
        obj is AuditableEntity auditableEntity &&
        Equals(auditableEntity);

    public override int GetHashCode() =>
        HashCode.Combine(
            Id.GetHashCode(),
            GetType().GetHashCode()) * 47;
}
