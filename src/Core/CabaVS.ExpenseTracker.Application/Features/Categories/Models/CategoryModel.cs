using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Models;

public sealed record CategoryModel(Guid Id, CategoryType Type, string Name, CurrencyModel Currency);