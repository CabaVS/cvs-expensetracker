using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyName : ValueObject
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    private CurrencyName(string value) => 
        Value = value;

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<CurrencyName> Create(string currencyName) =>
        Result<string>.Success(currencyName)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.CurrencyNameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.CurrencyNameIsTooLong(currencyName))
            .Map(x => new CurrencyName(x));
}
