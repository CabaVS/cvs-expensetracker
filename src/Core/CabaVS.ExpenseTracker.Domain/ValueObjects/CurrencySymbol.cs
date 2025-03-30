using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencySymbol : ValueObject
{
    public const int MaxLength = 4;
    
    public string Value { get; }

    private CurrencySymbol(string value) => 
        Value = value;

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<CurrencySymbol> Create(string currencySymbol) =>
        Result<string>.Success(currencySymbol)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.CurrencySymbolIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.CurrencySymbolIsTooLong(currencySymbol))
            .Map(x => new CurrencySymbol(x));
}
