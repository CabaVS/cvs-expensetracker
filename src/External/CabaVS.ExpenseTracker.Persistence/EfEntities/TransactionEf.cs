using CabaVS.ExpenseTracker.Domain.Entities;
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
    
    internal static TransactionEf FromDomain(Transaction transaction, Guid workspaceId)
    {
        var transactionEf = new TransactionEf
        {
            Id = transaction.Id,
            CreatedOn = transaction.CreatedOn,
            ModifiedOn = transaction.ModifiedOn,
            Date = transaction.Date,
            Type = transaction.Type,
            Tags = [.. transaction.Tags],
            AmountInSourceCurrency = transaction.AmountInSourceCurrency,
            AmountInDestinationCurrency = transaction.AmountInDestinationCurrency,
            WorkspaceId = workspaceId
        };

        switch (transaction.Type)
        {
            case TransactionType.Expense:
                transactionEf.SourceBalanceId = transaction.Source.Id;
                transactionEf.DestinationCategoryId = transaction.Destination.Id;
                break;
            case TransactionType.Income:
                transactionEf.SourceCategoryId = transaction.Source.Id;
                transactionEf.DestinationBalanceId = transaction.Destination.Id;
                break;
            case TransactionType.Transfer:
                transactionEf.SourceBalanceId = transaction.Source.Id;
                transactionEf.DestinationBalanceId = transaction.Destination.Id;
                break;
            default:
                throw new InvalidOperationException();
        }
        
        return transactionEf;
    }
}
