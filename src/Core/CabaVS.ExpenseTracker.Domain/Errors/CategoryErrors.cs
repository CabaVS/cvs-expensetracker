using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class CategoryErrors
{
    public static Error NotFoundById(Guid categoryId) => CommonErrors.NotFoundById(nameof(Category), categoryId);
    
    public static Error NameIsNullOrWhitespace() => StringErrors.IsNullOrWhiteSpace(nameof(Category), nameof(Category.Name));
    public static Error NameIsTooLong(string actual) => StringErrors.IsTooLong(nameof(Category), nameof(Category.Name), CategoryName.MaxLength, actual);
}
