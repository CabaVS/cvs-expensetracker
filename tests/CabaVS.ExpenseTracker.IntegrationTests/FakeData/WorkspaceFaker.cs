using Bogus;
using Bogus.Extensions;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class WorkspaceFaker : Faker<Workspace>
{
    public override Workspace Generate(string? ruleSets = null)
    {
        return new Workspace
        {
            Id = Guid.NewGuid(),
            Name = FakerHub.Company.CompanyName().ClampLength(max: WorkspaceName.MaxLength)
        };
    }
}