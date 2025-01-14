using System.Diagnostics.CodeAnalysis;

namespace CabaVS.ExpenseTracker.Domain.Primitives;

[SuppressMessage(
    "Major Code Smell",
    "S4035:Classes implementing \"IEquatable<T>\" should be sealed",
    Justification = "Target type taken into account.")]
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetAtomicValues();
    
    public static bool operator ==(ValueObject? first, ValueObject? second) =>
        first is not null &&
        first.Equals(second);

    public static bool operator !=(ValueObject? first, ValueObject? second) => !(first == second);

    public bool Equals(ValueObject? other) =>
        other is not null && 
        GetType() == other.GetType() &&
        ValuesAreEqual(other);

    public override bool Equals(object? obj) =>
        obj is ValueObject other && 
        GetType() == other.GetType() &&
        ValuesAreEqual(other);

    public override int GetHashCode() =>
        HashCode.Combine(
            GetAtomicValues()
                .Aggregate(
                    default(int),
                    HashCode.Combine),
            GetType().GetHashCode()) * 47;

    private bool ValuesAreEqual(ValueObject other) => GetAtomicValues().SequenceEqual(other.GetAtomicValues());
}
