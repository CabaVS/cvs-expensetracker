using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Application.Common.Errors;

public static class AdminAccessErrors
{
    public static Error NotAdmin() =>
        new(
            "AdminAccess.Required",
            "Admin access required for this operation.");
}