using System.Diagnostics.CodeAnalysis;

namespace CabaVS.ExpenseTracker.Domain.Primitives;

[SuppressMessage(
    "Major Code Smell",
    "S4035:Classes implementing \"IEquatable<T>\" should be sealed",
    Justification = "Target type is taken into account.")]
public abstract class AuditableEntity(Guid id, DateTime createdOn, DateTime modifiedOn) : IEquatable<AuditableEntity>
{
    public Guid Id { get; } = id;
    public DateTime CreatedOn { get; } = createdOn;
    public DateTime ModifiedOn { get; protected set; } = modifiedOn;

    public static bool operator ==(AuditableEntity? first, AuditableEntity? second) =>
        first is not null &&
        first.Equals(second);

    public static bool operator !=(AuditableEntity? first, AuditableEntity? second) => !(first == second);

    public bool Equals(AuditableEntity? other) =>
        other is not null && 
        other.GetType() == GetType() &&
        other.Id == Id;

    public override bool Equals(object? obj) =>
        obj is not null && 
        obj.GetType() == GetType() &&
        obj is AuditableEntity auditableEntity &&
        auditableEntity.Id == Id;

    public override int GetHashCode() =>
        HashCode.Combine(
            Id.GetHashCode(),
            GetType().GetHashCode()) * 47;
}
