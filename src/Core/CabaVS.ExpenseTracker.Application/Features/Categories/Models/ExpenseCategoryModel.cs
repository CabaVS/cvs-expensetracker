using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Models;

public sealed record ExpenseCategoryModel(Guid Id, string Name, CurrencyModel Currency);