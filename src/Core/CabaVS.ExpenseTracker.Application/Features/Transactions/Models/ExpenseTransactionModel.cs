namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record ExpenseTransactionModel(
    Guid Id,
    DateOnly Date,
    BalanceUnderTransactionModel Source,
    decimal AmountInSourceCurrency,
    CategoryUnderTransactionModel Destination,
    decimal AmountInDestinationCurrency,
    string[] Tags);