using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record IncomeTransactionModel(
    Guid Id,
    DateOnly Date,
    IncomeCategoryModel Source,
    decimal AmountInSourceCurrency,
    BalanceModel Destination,
    decimal AmountInDestinationCurrency,
    string[] Tags);