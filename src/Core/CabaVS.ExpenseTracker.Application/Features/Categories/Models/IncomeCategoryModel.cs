using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Models;

public sealed record IncomeCategoryModel(Guid Id, string Name)
{
    public required CurrencyModel Currency { get; init; }
}