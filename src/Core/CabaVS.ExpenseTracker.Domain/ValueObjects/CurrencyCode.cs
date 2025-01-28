using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public const int MaxLength = 4;
    
    public string Value { get; }

    private CurrencyCode(string value) => Value = value;

    public static Result<CurrencyCode> Create(string value) =>
        Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.CodeIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.CodeIsTooLong(value))
            .Map(x => new CurrencyCode(x));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
