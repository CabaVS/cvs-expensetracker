using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CurrencyErrors
{
    public static Error CurrencyCodeIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Code));
    public static Error CurrencyCodeIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(Currency), nameof(Currency.Code), CurrencyCode.MaxLength, actualValue);
    public static Error CurrencyCodeIsTooShort(string actualValue) =>
        StringErrors.IsTooShort(nameof(Currency), nameof(Currency.Code), CurrencyCode.MinLength, actualValue);
    
    public static Error CurrencyNameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Name));
    public static Error CurrencyNameIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(Currency), nameof(Currency.Name), CurrencyName.MaxLength, actualValue);
    
    public static Error CurrencySymbolIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Symbol));
    public static Error CurrencySymbolIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(Currency), nameof(Currency.Symbol), CurrencySymbol.MaxLength, actualValue);
    
    public static Error CurrencyNotFoundById(Guid id) =>
        CommonErrors.NotFoundById(nameof(Currency), id);
}
