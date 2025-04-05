using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Models;

public sealed record CategoryModel(Guid Id, string Name, CategoryType Type, CurrencySlimModel Currency);
