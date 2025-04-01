using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Models;

public sealed record BalanceModel(Guid Id, string Name, decimal Amount, CurrencySlimModel Currency);
