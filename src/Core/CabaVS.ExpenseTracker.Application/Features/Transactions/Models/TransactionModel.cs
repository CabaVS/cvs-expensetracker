using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record TransactionModel<TSource, TDestination>(
    Guid Id,
    TransactionType Type,
    DateOnly Date,
    TSource Source,
    decimal AmountInSourceCurrency,
    TDestination Destination,
    decimal AmountInDestinationCurrency,
    string[] Tags);