using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class CurrencyEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    
    internal Currency ToDomain() =>
        Currency
            .CreateExisting(
                Id,
                CreatedOn,
                ModifiedOn,
                Name,
                Code,
                Symbol)
            .Value;
}
