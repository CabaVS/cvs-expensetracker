using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class BalanceErrors
{
    public static Error BalanceNameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Balance), nameof(Balance.Name));
    
    public static Error BalanceNameIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(Balance), nameof(Balance.Name), BalanceName.MaxLength, actualValue);
    
    public static Error BalanceNotFoundById(Guid id) =>
        CommonErrors.NotFoundById(nameof(Balance), id);
}
