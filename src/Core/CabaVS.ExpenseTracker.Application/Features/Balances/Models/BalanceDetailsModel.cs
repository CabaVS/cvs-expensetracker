using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Models;

public sealed record BalanceDetailsModel(Guid Id, string Name, CurrencySlimModel Currency);
