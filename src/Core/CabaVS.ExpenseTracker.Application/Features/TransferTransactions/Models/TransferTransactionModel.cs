namespace CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Models;

public sealed record TransferTransactionModel(
    Guid Id,
    DateOnly Date,
    string[] Tags,
    decimal Amount,
    TransferTransactionModel.CurrencyModel Currency,
    decimal AmountInSourceCurrency,
    TransferTransactionModel.BalanceModel Source,
    decimal AmountInDestinationCurrency,
    TransferTransactionModel.BalanceModel Destination)
{
    public sealed record CurrencyModel(Guid Id, string Code);
    public sealed record BalanceModel(Guid Id, string Name, CurrencyModel Currency);
}