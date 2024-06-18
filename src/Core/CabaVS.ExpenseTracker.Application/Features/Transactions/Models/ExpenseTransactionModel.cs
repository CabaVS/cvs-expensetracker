using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record ExpenseTransactionModel(
    Guid Id,
    DateOnly Date,
    BalanceModel Source,
    decimal AmountInSourceCurrency,
    ExpenseCategoryModel Destination,
    decimal AmountInDestinationCurrency,
    string[] Tags);