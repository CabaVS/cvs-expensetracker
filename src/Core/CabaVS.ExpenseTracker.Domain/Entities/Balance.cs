using CabaVS.ExpenseTracker.Domain.Abstractions;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : AuditableEntity, IWithCurrency
{
    public BalanceName Name { get; }
    public decimal Amount { get; set; }
    public Currency Currency { get; }
    
    private Balance(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        BalanceName name, decimal amount, Currency currency) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
    }

    public Result<Transaction> CreateExpenseTransaction(
        DateOnly date, IEnumerable<string> tags,
        decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        Category destination) =>
        Transaction.CreateNew(
            date, TransactionType.Expense, tags,
            amountInSourceCurrency, amountInDestinationCurrency,
            this, destination);
    
    public Result<Transaction> CreateTransferTransaction(
        DateOnly date, IEnumerable<string> tags,
        decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        Balance destination) =>
        Transaction.CreateNew(
            date, TransactionType.Transfer, tags,
            amountInSourceCurrency, amountInDestinationCurrency,
            this, destination);

    public static Result<Balance> CreateNew(string name, decimal amount, Currency currency)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            name, amount, currency);
    }

    public static Result<Balance> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string name, decimal amount, Currency currency) =>
        BalanceName.Create(name)
            .Map(x => new Balance(id, createdOn, modifiedOn, x, amount, currency));
}
