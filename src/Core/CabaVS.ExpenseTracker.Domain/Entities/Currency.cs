using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Currency : Entity
{
    public CurrencyName Name { get; }
    public CurrencyCode Code { get; }
    public CurrencySymbol Symbol { get; }
    
    private Currency(
        Guid id, DateTime createdOn, DateTime? modifiedOn, 
        CurrencyName name, CurrencyCode code, CurrencySymbol symbol) 
        : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Code = code;
        Symbol = symbol;
    }
    
    public static Result<Currency> Create(string name, string code, string symbol) => 
        Create(Guid.NewGuid(), DateTime.UtcNow, null, name, code, symbol);

    public static Result<Currency> Create(Guid id, DateTime createdOn, DateTime? modifiedOn, string name, string code, string symbol)
    {
        Result<CurrencyName> nameResult = CurrencyName.Create(name);
        if (nameResult.IsFailure)
        {
            return nameResult.Error;
        }

        Result<CurrencyCode> codeResult = CurrencyCode.Create(code);
        if (codeResult.IsFailure)
        {
            return codeResult.Error;
        }

        Result<CurrencySymbol> symbolResult = CurrencySymbol.Create(symbol);
        return symbolResult.IsSuccess 
            ? new Currency(id, createdOn, modifiedOn, nameResult.Value, codeResult.Value, symbolResult.Value)
            : symbolResult.Error;
    }
}
