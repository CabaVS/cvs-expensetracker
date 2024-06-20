using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : Entity
{
    public BalanceName Name { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; }
    
    private Balance(Guid id, BalanceName name, decimal amount, Currency currency) : base(id)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
    }
    
    public static Result<Balance> Create(Guid id, string name, decimal amount, Currency currency)
    {
        var nameResult = BalanceName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new Balance(id, nameResult.Value, amount, currency);
    }

    public void ApplyTransaction(ExpenseTransaction expenseTransaction)
    {
        if (expenseTransaction.Source != this) throw NotLinkedTransactionException;

        Amount -= expenseTransaction.AmountInSourceCurrency;
    }
    
    public void ApplyTransaction(IncomeTransaction incomeTransaction)
    {
        if (incomeTransaction.Destination != this) throw NotLinkedTransactionException;

        Amount += incomeTransaction.AmountInDestinationCurrency;
    }
    
    public void ApplyTransaction(TransferTransaction transferTransaction)
    {
        if (transferTransaction.Source == this)
            Amount -= transferTransaction.AmountInSourceCurrency;
        else if (transferTransaction.Destination == this)
            Amount += transferTransaction.AmountInDestinationCurrency;
        else
            throw NotLinkedTransactionException;
    }
    
    public void RollbackTransaction(ExpenseTransaction expenseTransaction)
    {
        if (expenseTransaction.Source != this) throw NotLinkedTransactionException;

        Amount += expenseTransaction.AmountInSourceCurrency;
    }
    
    public void RollbackTransaction(IncomeTransaction incomeTransaction)
    {
        if (incomeTransaction.Destination != this) throw NotLinkedTransactionException;

        Amount -= incomeTransaction.AmountInDestinationCurrency;
    }
    
    public void RollbackTransaction(TransferTransaction transferTransaction)
    {
        if (transferTransaction.Source == this)
            Amount += transferTransaction.AmountInSourceCurrency;
        else if (transferTransaction.Destination == this)
            Amount -= transferTransaction.AmountInDestinationCurrency;
        else
            throw NotLinkedTransactionException;
    }
    
    private static readonly InvalidOperationException NotLinkedTransactionException =
        new("Unable to apply migration that is not linked to this Balance.");
}