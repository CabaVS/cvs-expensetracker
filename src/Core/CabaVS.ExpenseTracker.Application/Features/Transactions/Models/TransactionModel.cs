using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Models;

public record TransactionModel(Guid Id, DateTime CreatedOn, DateTime ModifiedOn, 
    DateOnly Date, string[] Tags, decimal AmountInSourceCurrency, decimal AmountInDestinationCurrency,
    TransactionType Type, object Source, object Destination);
