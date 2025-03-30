using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public const int MinLength = 3;
    public const int MaxLength = 3;
    
    public string Value { get; }

    private CurrencyCode(string value) => 
        Value = value;

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<CurrencyCode> Create(string currencyCode) =>
        Result<string>.Success(currencyCode)
            .EnsureStringNotNullOrWhitespace(CurrencyErrors.CurrencyCodeIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CurrencyErrors.CurrencyCodeIsTooLong(currencyCode))
            .EnsureStringNotTooShort(MinLength, CurrencyErrors.CurrencyCodeIsTooShort(currencyCode))
            .Map(x => new CurrencyCode(x));
}
