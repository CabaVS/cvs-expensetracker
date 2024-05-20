using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencySymbol : ValueObject
{
    public const int MaxLength = 4;
    
    public string Value { get; }

    private CurrencySymbol(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<CurrencySymbol> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return CurrencyErrors.SymbolIsNullOrWhitespace();
        if (value.Length > MaxLength) return CurrencyErrors.SymbolTooLong(value);
        
        return new CurrencySymbol(value);
    }
}