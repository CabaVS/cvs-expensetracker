namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record IncomeTransactionModel(
    Guid Id,
    DateOnly Date,
    CategoryUnderTransactionModel Source,
    decimal AmountInSourceCurrency,
    BalanceUnderTransactionModel Destination,
    decimal AmountInDestinationCurrency,
    string[] Tags);