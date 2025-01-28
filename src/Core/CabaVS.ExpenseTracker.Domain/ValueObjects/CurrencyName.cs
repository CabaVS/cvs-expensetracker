using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private CurrencyName(string value) => Value = value;

    public static Result<CurrencyName> Create(string value) =>
        Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.NameIsTooLong(value))
            .Map(x => new CurrencyName(x));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
