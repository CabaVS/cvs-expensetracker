using Bogus;
using Bogus.Extensions;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class BalanceFaker(Guid workspaceId, Guid currencyId) : Faker<Balance>
{
    public override Balance Generate(string? ruleSets = null)
    {
        return new Balance
        {
            Id = Guid.NewGuid(),
            Name = FakerHub.Finance.AccountName().ClampLength(BalanceName.MaxLength),
            Amount = FakerHub.Finance.Amount(),
            WorkspaceId = workspaceId,
            CurrencyId = currencyId
        };
    }
}