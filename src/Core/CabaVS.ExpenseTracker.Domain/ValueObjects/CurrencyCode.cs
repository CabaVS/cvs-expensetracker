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
        if (string.IsNullOrWhiteSpace(value)) return CurrencyErrors.CodeIsEmptyOrWhitespace();
        
        return value.Length switch
        {
            > MaxLength => CurrencyErrors.CodeTooLong(value),
            < MinLength => CurrencyErrors.CodeTooShort(value),
            _ => new CurrencyCode(value)
        };
    }
}