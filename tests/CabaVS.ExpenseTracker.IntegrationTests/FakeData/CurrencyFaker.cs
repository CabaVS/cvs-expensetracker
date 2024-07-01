using Bogus;
using Bogus.Extensions;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class CurrencyFaker : Faker<Currency>
{
    public override Currency Generate(string? ruleSets = null)
    {
        var currency = FakerHub.Finance.Currency();
        return new Currency
        {
            Id = Guid.NewGuid(),
            Name = currency.Description.ClampLength(max: CurrencyName.MaxLength),
            Code = currency.Code.ClampLength(max: CurrencyCode.MaxLength),
            Symbol = currency.Symbol.ClampLength(min: CurrencyCode.MinLength, max: CurrencyCode.MaxLength)
        };
    }
}