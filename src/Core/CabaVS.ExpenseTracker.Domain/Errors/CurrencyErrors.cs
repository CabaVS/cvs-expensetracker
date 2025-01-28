using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CurrencyErrors
{
    public static Error NotFoundById(Guid currencyId) => CommonErrors.NotFoundById(nameof(Currency), currencyId);
    
    public static Error NameIsNullOrWhitespace() => StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Name));
    public static Error NameIsTooLong(string actual) => StringErrors.IsTooLong(nameof(Currency), nameof(Currency.Name), CurrencyName.MaxLength, actual);
    
    public static Error CodeIsNullOrWhitespace() => StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Code));
    public static Error CodeIsTooLong(string actual) => StringErrors.IsTooLong(nameof(Currency), nameof(Currency.Code), CurrencyCode.MaxLength, actual);
    
    public static Error SymbolIsNullOrWhitespace() => StringErrors.IsNullOrWhiteSpace(nameof(Currency), nameof(Currency.Symbol));
    public static Error SymbolIsTooLong(string actual) => StringErrors.IsTooLong(nameof(Currency), nameof(Currency.Symbol), CurrencySymbol.MaxLength, actual);
}
