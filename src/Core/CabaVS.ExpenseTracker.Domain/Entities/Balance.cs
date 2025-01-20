using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : Entity
{
    public BalanceName Name { get; private set; }
    public decimal Amount { get; private set; }
    
    public Currency Currency { get; }

    private Balance(
        Guid id, DateTime createdOn, DateTime? modifiedOn, 
        BalanceName name, decimal amount, Currency currency) 
        : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
    }

    public static Result<Balance> Create(string name, decimal amount, Currency currency) => 
        Create(Guid.NewGuid(), DateTime.UtcNow, null, name, amount, currency); 
    
    public static Result<Balance> Create(
        Guid id, DateTime createdOn, DateTime? modifiedOn, 
        string name, decimal amount, Currency currency)
    {
        Result<BalanceName> nameResult = BalanceName.Create(name);
        return nameResult.IsSuccess
            ? new Balance(id, createdOn, modifiedOn, nameResult.Value, amount, currency)
            : nameResult.Error;
    }

    public void ApplyTransferTransaction(TransferTransaction transferTransaction, bool isSource)
    {
        ArgumentNullException.ThrowIfNull(transferTransaction);
        
        if (isSource)
        {
            Amount -= transferTransaction.AmountInSourceCurrency;
        }
        else
        {
            Amount += transferTransaction.AmountInDestinationCurrency;
        }

        ModifiedOn = DateTime.UtcNow;
    }

    public Result UpdateName(string name)
    {
        Result<BalanceName> nameResult = BalanceName.Create(name);
        if (nameResult.IsFailure)
        {
            return nameResult.Error;
        }
        
        Name = nameResult.Value;
        ModifiedOn = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdateAmount(decimal amount)
    {
        Amount = amount;
        ModifiedOn = DateTime.UtcNow;
        
        return Result.Success();
    }
}
