using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CategoryErrors
{
    public static Error NameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace("Category", "Name");
    public static Error NameTooLong(string actualValue) =>
        StringErrors.TooLong("Category", "Name", CategoryName.MaxLength, actualValue);
}