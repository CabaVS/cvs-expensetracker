namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record TransactionMoneyByCategoryModel(
    Guid CategoryId,
    string CategoryName,
    decimal Amount,
    string CurrencyCode)
{
    public TransactionMoneyByTagModel[] ByTag { get; set; } = [];
}
