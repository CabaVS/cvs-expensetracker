using Bogus;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.IntegrationTests.Extensions;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class BalanceFaker(Guid currencyId, Guid workspaceId) : Faker<Balance>
{
    public override Balance Generate(string? ruleSets = null)
    {
        DateTime modifiedOn = FakerHub.Date.Past();
        DateTime createdOn = FakerHub.Date.Past(yearsToGoBack: 2, refDate: modifiedOn);

        return new Balance
        {
            Id = FakerHub.Random.Guid(),
            CreatedOn = createdOn,
            ModifiedOn = modifiedOn,
            Name = FakerHub.Finance.AccountName().TakeFirstChars(BalanceName.MaxLength),
            Amount = FakerHub.Finance.Amount(),
            CurrencyId = currencyId,
            WorkspaceId = workspaceId
        };
    }
}
