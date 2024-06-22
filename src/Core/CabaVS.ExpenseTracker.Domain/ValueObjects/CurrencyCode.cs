using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public const int MaxLength = 3;
    public const int MinLength = 3;
    
    public string Value { get; }

    private CurrencyCode(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<CurrencyCode> Create(string value)
    {
        return Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.CodeIsEmptyOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.CodeTooLong(value))
            .EnsureStringNotTooShort(MinLength, CurrencyErrors.CodeTooShort(value))
            .Map(x => new CurrencyCode(x));
    }
}