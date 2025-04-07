using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class TransactionErrors
{
    public static Error SourceAndDestinationAreSame() =>
        new(
            $"{nameof(Transaction)}.{nameof(SourceAndDestinationAreSame)}", 
            "Source and destination are same entities.");
    
    public static Error SourceOrDestinationMismatch() =>
        new(
            $"{nameof(Transaction)}.{nameof(SourceOrDestinationMismatch)}", 
            "Source or destination mismatch for the transaction with provided Type.");
    
    public static Error SourceOrDestinationNotFound() =>
        new(
            $"{nameof(Transaction)}.{nameof(SourceOrDestinationNotFound)}", 
            "Source or destination not found for the transaction with provided Type.");
    
    public static Error AmountsMustDifferWhenCurrenciesAreDifferent() =>
        new(
            $"{nameof(Transaction)}.{nameof(AmountsMustDifferWhenCurrenciesAreDifferent)}", 
            "Amounts must differ when currencies are different for source and destination.");
    
    public static Error NotFoundById(Guid transactionId) =>
        CommonErrors.NotFoundById(nameof(Transaction), transactionId);
}
