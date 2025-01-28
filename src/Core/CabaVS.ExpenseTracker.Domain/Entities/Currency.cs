using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Currency : AuditableEntity
{
    public CurrencyName Name { get; }
    public CurrencyCode Code { get; }
    public CurrencySymbol Symbol { get; }

    private Currency(Guid id, DateTime createdOn, DateTime modifiedOn,
        CurrencyName name, CurrencyCode code, CurrencySymbol symbol) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Code = code;
        Symbol = symbol;
    }
    
    public static Result<Currency> Create(string name, string code, string symbol)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return Create(Guid.NewGuid(), utcNow, utcNow, name, code, symbol);
    }

    public static Result<Currency> Create(Guid id, DateTime createdOn, DateTime modifiedOn,
        string name, string code, string symbol)
    {
        Result<CurrencyName> currencyNameResult = CurrencyName.Create(name);
        if (currencyNameResult.IsFailure)
        {
            return currencyNameResult.Error;
        }
        
        Result<CurrencyCode> currencyCodeResult = CurrencyCode.Create(code);
        if (currencyCodeResult.IsFailure)
        {
            return currencyCodeResult.Error;
        }
        
        Result<CurrencySymbol> currencySymbolResult = CurrencySymbol.Create(symbol);
        if (currencySymbolResult.IsFailure)
        {
            return currencySymbolResult.Error;
        }
        
        var currency = new Currency(id, createdOn, modifiedOn, 
            currencyNameResult.Value, currencyCodeResult.Value, currencySymbolResult.Value);
        return currency;
    }
}
