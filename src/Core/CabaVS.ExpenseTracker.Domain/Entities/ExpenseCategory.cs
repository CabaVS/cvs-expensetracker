using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class ExpenseCategory : Entity
{
    public CategoryName Name { get; set; }

    public Currency Currency { get; }
    
    private ExpenseCategory(Guid id, CategoryName name, Currency currency) : base(id)
    {
        Name = name;
        Currency = currency;
    }

    public static Result<ExpenseCategory> Create(Guid id, string name, Currency currency)
    {
        Result<CategoryName> nameResult = CategoryName.Create(name);
        return nameResult.IsFailure ? nameResult.Error : new ExpenseCategory(id, nameResult.Value, currency);
    }
}
