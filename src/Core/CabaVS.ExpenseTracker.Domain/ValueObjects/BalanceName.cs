using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class BalanceName : ValueObject
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    private BalanceName(string value) => 
        Value = value;

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<BalanceName> Create(string balanceName) =>
        Result<string>.Success(balanceName)
            .EnsureStringNotNullOrWhitespace(BalanceErrors.BalanceNameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, BalanceErrors.BalanceNameIsTooLong(balanceName))
            .Map(x => new BalanceName(x));
}
