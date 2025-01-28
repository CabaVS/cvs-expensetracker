using Bogus;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class CurrencyFaker : Faker<Currency>
{
    public override Currency Generate(string? ruleSets = null)
    {
        DateTime modifiedOn = FakerHub.Date.Past();
        DateTime createdOn = FakerHub.Date.Past(refDate: modifiedOn);

        Bogus.DataSets.Currency? currency = FakerHub.Finance.Currency();
        
        return new Currency
        {
            Id = FakerHub.Random.Guid(),
            CreatedOn = createdOn,
            ModifiedOn = modifiedOn,
            Name = currency.Description,
            Code = currency.Code,
            Symbol = currency.Symbol
        };
    }
}
