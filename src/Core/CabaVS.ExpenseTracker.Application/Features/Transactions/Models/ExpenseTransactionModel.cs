using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record ExpenseTransactionModel(
    Guid Id,
    DateOnly Date,
    decimal AmountInSourceCurrency,
    decimal AmountInDestinationCurrency)
{
    public required BalanceModel Source { get; init; }
    public required ExpenseCategoryModel Destination { get; init; }
}