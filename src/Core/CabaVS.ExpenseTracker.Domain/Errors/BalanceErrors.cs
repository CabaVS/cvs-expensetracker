using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class BalanceErrors
{
    public static Error NotFoundById(Guid balanceId) => CommonErrors.NotFoundById(nameof(Balance), balanceId);
    
    public static Error NameIsNullOrWhitespace() => StringErrors.IsNullOrWhiteSpace(nameof(Balance), nameof(Balance.Name));
    public static Error NameIsTooLong(string actual) => StringErrors.IsTooLong(nameof(Balance), nameof(Balance.Name), BalanceName.MaxLength, actual);
}
