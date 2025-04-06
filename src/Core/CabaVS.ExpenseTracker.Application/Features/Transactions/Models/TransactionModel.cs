using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public sealed record TransactionModel(
    Guid Id,
    DateOnly Date,
    TransactionType Type,
    string[] Tags,
    decimal AmountInSourceCurrency,
    decimal AmountInDestinationCurrency,
    TransactionSideModel Source,
    TransactionSideModel Destination);
