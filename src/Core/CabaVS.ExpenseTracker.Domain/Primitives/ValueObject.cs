namespace CabaVS.ExpenseTracker.Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> AtomicValues { get; }
    
    public static bool operator ==(ValueObject? first, ValueObject? second) =>
        first is not null &&
        first.Equals(second);

    public static bool operator !=(ValueObject? first, ValueObject? second) =>
        !(first == second);
    
    public bool Equals(ValueObject? other) =>
        other is not null && 
        GetType() == other.GetType() &&
        AtomicValues.SequenceEqual(other.AtomicValues);

    public override bool Equals(object? obj) =>
        obj is ValueObject other && 
        Equals(other);
    
    public override int GetHashCode() =>
        HashCode.Combine(
            AtomicValues.Aggregate(0, HashCode.Combine),
            GetType().GetHashCode()) * 47;
}