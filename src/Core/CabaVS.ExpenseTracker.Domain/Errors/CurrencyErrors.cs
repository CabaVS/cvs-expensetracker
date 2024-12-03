using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CurrencyErrors
{
    public static Error NameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Name));
    public static Error NameTooLong(string actualValue) =>
        StringErrors.TooLong(nameof(Currency), nameof(Currency.Name), CurrencyName.MaxLength, actualValue);
    
    public static Error CodeIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Code));
    public static Error CodeTooLong(string actualValue) =>
        StringErrors.TooLong(nameof(Currency), nameof(Currency.Code), CurrencyCode.MaxLength, actualValue);
    
    public static Error SymbolIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Symbol));
    public static Error SymbolTooLong(string actualValue) =>
        StringErrors.TooLong(nameof(Currency), nameof(Currency.Symbol), CurrencySymbol.MaxLength, actualValue);
    
    public static Error NotFoundById(Guid id) =>
        CommonErrors.NotFoundById(nameof(Currency), id);
}