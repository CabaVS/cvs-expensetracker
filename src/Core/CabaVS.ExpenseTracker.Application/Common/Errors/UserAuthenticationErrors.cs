using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Application.Common.Errors;

internal static class UserAuthenticationErrors
{
    public static Error NotAuthenticated() => new("User.NotAuthenticated", "User is not authenticated.");
}