using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class UserErrors
{
    public static Error UserNameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(User), nameof(User.UserName));
    
    public static Error UserNameIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(User), nameof(User.UserName), UserName.MaxLength, actualValue);
}