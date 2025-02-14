using Bogus;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.IntegrationTests.Extensions;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class CurrencyFaker : Faker<Currency>
{
    public override Currency Generate(string? ruleSets = null)
    {
        DateTime modifiedOn = FakerHub.Date.Past();
        DateTime createdOn = FakerHub.Date.Past(yearsToGoBack: 2, refDate: modifiedOn);

        Bogus.DataSets.Currency? currency = FakerHub.Finance.Currency();
        
        return new Currency
        {
            Id = FakerHub.Random.Guid(),
            CreatedOn = createdOn,
            ModifiedOn = modifiedOn,
            Name = currency.Description.TakeFirstChars(CurrencyName.MaxLength),
            Code = currency.Code.TakeFirstChars(CurrencyCode.MaxLength),
            Symbol = currency.Symbol.TakeFirstChars(CurrencySymbol.MaxLength)
        };
    }
}
