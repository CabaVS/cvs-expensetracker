using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class BalanceName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private BalanceName(string value) => Value = value;

    public static Result<BalanceName> Create(string value) =>
        Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(BalanceErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, BalanceErrors.NameIsTooLong(value))
            .Map(x => new BalanceName(x));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
