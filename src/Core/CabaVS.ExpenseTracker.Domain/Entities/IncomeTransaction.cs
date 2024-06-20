using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class IncomeTransaction : Entity
{
    public DateOnly Date { get; set; }
    
    public IncomeCategory Source { get; set; }
    public decimal AmountInSourceCurrency { get; set; }
    
    public Balance Destination { get; private set; }
    public decimal AmountInDestinationCurrency { get; private set; }
    
    public List<TransactionTag> Tags { get; set; }
    
    private IncomeTransaction(
        Guid id, 
        DateOnly date, 
        IncomeCategory source,
        decimal amountInSourceCurrency, 
        Balance destination,
        decimal amountInDestinationCurrency,
        List<TransactionTag>? tags = null) : base(id)
    {
        Date = date;

        Source = source;
        AmountInSourceCurrency = amountInSourceCurrency;
        
        Destination = destination;
        AmountInDestinationCurrency = amountInDestinationCurrency;
        
        Tags = tags ?? [];
    }

    public static Result<IncomeTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        IncomeCategory source,
        Balance destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency,
        IEnumerable<string>? tags = null,
        bool recalculateBalances = false)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        var tagsResult = TransactionTag.CreateMultiple(tags);
        if (tagsResult.IsFailure) return tagsResult.Error;

        if (recalculateBalances)
        {
            destination.Amount += amountInDestinationCurrency;
        }
        
        return new IncomeTransaction(id, dateInUtc, source, amountInSourceCurrency, destination, amountInDestinationCurrency);
    }

    public void ChangeDestination(Balance balance)
    {
        if (Destination == balance) return;

        Destination.Amount -= AmountInDestinationCurrency;
        Destination = balance;
        Destination.Amount += AmountInDestinationCurrency;
    }

    public void ChangeDestinationAmount(decimal amountInBalanceCurrency)
    {
        if (AmountInDestinationCurrency == amountInBalanceCurrency) return;

        Destination.Amount -= AmountInDestinationCurrency;
        AmountInDestinationCurrency = amountInBalanceCurrency;
        Destination.Amount += AmountInDestinationCurrency;
    }
}