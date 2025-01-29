using CabaVS.ExpenseTracker.Domain.Entities.Abstractions;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Category : AuditableEntity, IWithCurrency, IWithWorkspace
{
    public CategoryName Name { get; set; }

    public CategoryType Type { get; }

    public Currency Currency { get; }
    public Workspace Workspace { get; }
    
    private Category(Guid id, DateTime createdOn, DateTime modifiedOn,
        CategoryName name, CategoryType type, Currency currency, Workspace workspace) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Type = type;
        Currency = currency;
        Workspace = workspace;
    }
    
    public static Result<Category> Create(string name, CategoryType type, Currency currency, Workspace workspace) => 
        Create(Guid.NewGuid(), default, default, name, type, currency, workspace);

    public static Result<Category> Create(Guid id, DateTime createdOn, DateTime modifiedOn, 
        string name, CategoryType type, Currency currency, Workspace workspace)
    {
        Result<CategoryName> categoryNameResult = CategoryName.Create(name);
        if (categoryNameResult.IsFailure)
        {
            return categoryNameResult.Error;
        }
        
        var category = new Category(id, createdOn, modifiedOn, categoryNameResult.Value, type, currency, workspace);
        return category;
    }
}
