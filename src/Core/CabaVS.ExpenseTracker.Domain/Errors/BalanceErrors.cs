using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class BalanceErrors
{
    public static Error NameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Balance), nameof(Balance.Name));
    public static Error NameTooLong(string actualValue) =>
        StringErrors.TooLong(nameof(Balance), nameof(Balance.Name), BalanceName.MaxLength, actualValue);
}