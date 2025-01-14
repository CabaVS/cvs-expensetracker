using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Currency : Entity
{
    public CurrencyName Name { get; }
    public CurrencyCode Code { get; }
    public CurrencySymbol Symbol { get; }
    
    private Currency(Guid id, CurrencyName name, CurrencyCode code, CurrencySymbol symbol) : base(id)
    {
        Name = name;
        Code = code;
        Symbol = symbol;
    }

    public static Result<Currency> Create(Guid id, string name, string code, string symbol)
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
        return symbolResult.IsFailure ? symbolResult.Error : new Currency(id, nameResult.Value, codeResult.Value, symbolResult.Value);
    }
}
