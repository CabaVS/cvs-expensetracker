using Bogus;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.IntegrationTests.Extensions;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class WorkspaceFaker(Guid? id = null) : Faker<Workspace>
{
    public override Workspace Generate(string? ruleSets = null)
    {
        DateTime modifiedOn = FakerHub.Date.Past();
        DateTime createdOn = FakerHub.Date.Past(yearsToGoBack: 2, refDate: modifiedOn);
        
        return new Workspace
        {
            Id = id ?? Guid.NewGuid(),
            CreatedOn = createdOn,
            ModifiedOn = modifiedOn,
            Name = FakerHub.Random.Words(2).TakeFirstChars(WorkspaceName.MaxLength)
        };
    }
}
