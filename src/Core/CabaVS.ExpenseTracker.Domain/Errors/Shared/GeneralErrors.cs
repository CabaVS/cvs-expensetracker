using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Errors.Shared;

public static class GeneralErrors
{
    public static Error NotFoundById(string entityName, Guid id) =>
        new($"{entityName}.NotFoundById", $"{entityName} not found by ID '{id}'.");
}