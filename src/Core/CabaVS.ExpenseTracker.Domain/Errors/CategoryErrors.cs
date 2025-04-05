using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CategoryErrors
{
    public static Error NameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Category), nameof(Category.Name));
    
    public static Error NameIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(Category), nameof(Category.Name), CategoryName.MaxLength, actualValue);
    
    public static Error NotFoundById(Guid id) =>
        CommonErrors.NotFoundById(nameof(Category), id);
}
