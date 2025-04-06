using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class TransactionEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public DateOnly Date { get; set; }
    public TransactionType Type { get; set; }
    public string[] Tags { get; set; } = [];

    public decimal AmountInSourceCurrency { get; set; }
    public decimal AmountInDestinationCurrency { get; set; }
    
    public Guid WorkspaceId { get; set; }
    public WorkspaceEf? Workspace { get; set; }

    public Guid? SourceBalanceId { get; set; }
    public BalanceEf? SourceBalance { get; set; }
    
    public Guid? DestinationBalanceId { get; set; }
    public BalanceEf? DestinationBalance { get; set; }
    
    public Guid? SourceCategoryId { get; set; }
    public CategoryEf? SourceCategory { get; set; }
    
    public Guid? DestinationCategoryId { get; set; }
    public CategoryEf? DestinationCategory { get; set; }
}
