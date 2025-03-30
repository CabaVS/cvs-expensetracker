using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Currency : AuditableEntity
{
    public CurrencyCode Code { get; }
    public CurrencyName Name { get; }
    public CurrencySymbol Symbol { get; }
    
    private Currency(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        CurrencyName name, CurrencyCode code, CurrencySymbol symbol) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Code = code;
        Symbol = symbol;
    }

    public static Result<Currency> CreateNew(string name, string code, string symbol)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            name, code, symbol);
    }

    public static Result<Currency> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string name, string code, string symbol)
    {
        var listOfErrors = new List<Error>();
        
        Result<CurrencyName> nameResult = CurrencyName.Create(name);
        if (nameResult.IsFailure)
        {
            listOfErrors.Add(nameResult.Error);
        }
        
        Result<CurrencyCode> codeResult = CurrencyCode.Create(code);
        if (codeResult.IsFailure)
        {
            listOfErrors.Add(codeResult.Error);
        }
        
        Result<CurrencySymbol> symbolResult = CurrencySymbol.Create(symbol);
        if (symbolResult.IsFailure)
        {
            listOfErrors.Add(symbolResult.Error);
        }

        return listOfErrors is { Count: > 0 }
            ? Result<Currency>.Failure(
                new Error(
                    $"{nameof(Currency)}.ValidationFailed",
                    "One or more errors occurred during validation.",
                    [.. listOfErrors]))
            : Result<Currency>.Success(
                new Currency(id, createdOn, modifiedOn, nameResult.Value, codeResult.Value, symbolResult.Value));
    }
}
