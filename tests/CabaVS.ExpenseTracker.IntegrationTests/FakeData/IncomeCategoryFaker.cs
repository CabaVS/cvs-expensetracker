using Bogus;
using Bogus.Extensions;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class IncomeCategoryFaker(Guid workspaceId, Guid currencyId) : Faker<IncomeCategory>
{
    public override IncomeCategory Generate(string? ruleSets = null)
    {
        return new IncomeCategory
        {
            Id = Guid.NewGuid(),
            Name = FakerHub.Company.CompanyName().ClampLength(CategoryName.MaxLength),
            WorkspaceId = workspaceId,
            CurrencyId = currencyId
        };
    }
}