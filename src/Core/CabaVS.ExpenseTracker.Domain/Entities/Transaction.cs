using CabaVS.ExpenseTracker.Domain.Entities.Abstractions;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Transaction : AuditableEntity, IWithWorkspace
{
    public DateOnly Date { get; set; }
    public string[] Tags { get; set; }
    public decimal AmountInSourceCurrency { get; set; }
    public decimal AmountInDestinationCurrency { get; set; }

    public TransactionType Type { get; }
    public IWithCurrency Source { get; }
    public IWithCurrency Destination { get; }
    public Workspace Workspace { get; }
    
    private Transaction(Guid id, DateTime createdOn, DateTime modifiedOn,
        DateOnly date, string[] tags, decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        TransactionType type, IWithCurrency source, IWithCurrency destination, Workspace workspace) : base(id, createdOn, modifiedOn)
    {
        Date = date;
        Tags = tags;
        AmountInSourceCurrency = amountInSourceCurrency;
        AmountInDestinationCurrency = amountInDestinationCurrency;
        
        Type = type;
        Source = source;
        Destination = destination;
        Workspace = workspace;
    }
    
    public static Result<Transaction> CreateIncomeTransaction(
        DateOnly date, string[] tags, decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        Category source, Balance destination, Workspace workspace) =>
        Create(
            date, tags, amountInSourceCurrency, amountInDestinationCurrency,
            TransactionType.Income, source, destination, workspace);
    
    public static Result<Transaction> CreateExpenseTransaction(
        DateOnly date, string[] tags, decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        Balance source, Category destination, Workspace workspace) => 
        Create(
            date, tags, amountInSourceCurrency, amountInDestinationCurrency,
            TransactionType.Expense, source, destination, workspace);

    public static Result<Transaction> CreateTransferTransaction(
        DateOnly date, string[] tags, decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        Balance source, Balance destination, Workspace workspace) => 
        Create(
            date, tags, amountInSourceCurrency, amountInDestinationCurrency,
            TransactionType.Transfer, source, destination, workspace);

    public static Result<Transaction> Create(
        DateOnly date, string[] tags, decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        TransactionType type, IWithCurrency source, IWithCurrency destination, Workspace workspace) =>
        Create(Guid.NewGuid(), default, default, 
            date, tags, amountInSourceCurrency, amountInDestinationCurrency,
            type, source, destination, workspace);

    public static Result<Transaction> Create(Guid id, DateTime createdOn, DateTime modifiedOn,
        DateOnly date, string[] tags, decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        TransactionType type, IWithCurrency source, IWithCurrency destination, Workspace workspace)
    {
        ValidateSourceAndDestination(type, source, destination);
        
        var transaction = new Transaction(id, createdOn, modifiedOn,
            date, tags, amountInSourceCurrency, amountInDestinationCurrency, 
            type, source, destination, workspace);
        return transaction;
    }

    private static void ValidateSourceAndDestination(TransactionType type, IWithCurrency source, IWithCurrency destination)
    {
        var isValid = type switch
        {
            TransactionType.Income => source is Category { Type: CategoryType.Income } && destination is Balance,
            TransactionType.Expense => destination is Category { Type: CategoryType.Expense } && source is Balance,
            TransactionType.Transfer => source is Balance && destination is Balance,
            _ => throw new InvalidOperationException("Not supported value for transaction type.")
        };

        if (!isValid)
        {
            throw new InvalidOperationException("Transaction validation failed for source and/or destination.");
        }
    }
}
