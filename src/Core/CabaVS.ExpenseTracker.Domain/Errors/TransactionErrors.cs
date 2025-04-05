﻿using CabaVS.ExpenseTracker.Domain.Entities;
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
    
    public static Error AmountsMustDifferWhenCurrenciesAreDifferent() =>
        new(
            $"{nameof(Transaction)}.{nameof(AmountsMustDifferWhenCurrenciesAreDifferent)}", 
            "Amounts must differ when currencies are different for source and destination.");
}
