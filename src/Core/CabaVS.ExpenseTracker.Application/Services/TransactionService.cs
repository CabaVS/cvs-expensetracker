using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Application.Services;

internal interface ITransactionService
{
    void ApplyTransaction(Transaction transaction);
    void RollbackTransaction(Transaction transaction);
}

internal sealed class TransactionService : ITransactionService
{
    public void ApplyTransaction(Transaction transaction)
    {
        switch (transaction.Type)
        {
            case TransactionType.Income:
                {
                    ((Balance)transaction.Destination).Amount += transaction.AmountInDestinationCurrency;
                    break;
                }
            case TransactionType.Expense:
                {
                    ((Balance)transaction.Source).Amount -= transaction.AmountInSourceCurrency;
                    break;
                }
            case TransactionType.Transfer:
                {
                    ((Balance)transaction.Source).Amount -= transaction.AmountInSourceCurrency;
                    ((Balance)transaction.Destination).Amount += transaction.AmountInDestinationCurrency;
                    break;
                }
            default:
                throw new InvalidOperationException();
        }
    }
    
    public void RollbackTransaction(Transaction transaction)
    {
        switch (transaction.Type)
        {
            case TransactionType.Income:
                {
                    ((Balance)transaction.Destination).Amount -= transaction.AmountInDestinationCurrency;
                    break;
                }
            case TransactionType.Expense:
                {
                    ((Balance)transaction.Source).Amount += transaction.AmountInSourceCurrency;
                    break;
                }
            case TransactionType.Transfer:
                {
                    ((Balance)transaction.Source).Amount += transaction.AmountInSourceCurrency;
                    ((Balance)transaction.Destination).Amount -= transaction.AmountInDestinationCurrency;
                    break;
                }
            default:
                throw new InvalidOperationException();
        }
    }
}
