using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private CurrencyName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<CurrencyName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return CurrencyErrors.NameIsNullOrWhitespace();
        if (value.Length > MaxLength) return CurrencyErrors.NameTooLong(value);
        
        return new CurrencyName(value);
    }
}