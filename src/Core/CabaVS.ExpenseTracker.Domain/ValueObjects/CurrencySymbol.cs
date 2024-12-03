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

    public static Result<CurrencySymbol> Create(string value)
    {
        return Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.SymbolIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.SymbolTooLong(value))
            .Map(x => new CurrencySymbol(x));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}