using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class ExpenseCategory : Entity
{
    public CategoryName Name { get; private set; }

    public Currency Currency { get; }
    
    private ExpenseCategory(
        Guid id, DateTime createdOn, DateTime? modifiedOn, 
        CategoryName name, Currency currency) 
        : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Currency = currency;
    }
    
    public static Result<ExpenseCategory> Create(string name, Currency currency) =>
        Create(Guid.NewGuid(), DateTime.UtcNow, null, name, currency);

    public static Result<ExpenseCategory> Create(
        Guid id, DateTime createdOn, DateTime? modifiedOn, 
        string name, Currency currency)
    {
        Result<CategoryName> nameResult = CategoryName.Create(name);
        return nameResult.IsSuccess 
            ? new ExpenseCategory(id, createdOn, modifiedOn, nameResult.Value, currency)
            : nameResult.Error;
    }
}
