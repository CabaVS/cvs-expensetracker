using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record IncomeTransactionModel(
    Guid Id,
    DateOnly Date,
    decimal AmountInSourceCurrency,
    decimal AmountInDestinationCurrency)
{
    public required IncomeCategoryModel Source { get; init; }
    public required BalanceModel Destination { get; init; }
}