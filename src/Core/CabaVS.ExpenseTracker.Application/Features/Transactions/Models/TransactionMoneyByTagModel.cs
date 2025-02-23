namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record TransactionMoneyByTagModel(
    string[] Tags,
    decimal Amount,
    string CurrencyCode);
