using Bogus;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class WorkspaceFaker(Guid? id = null) : Faker<Workspace>
{
    public override Workspace Generate(string? ruleSets = null)
    {
        var generatedName = FakerHub.Random.Words(2);
        
        return new Workspace
        {
            Id = id ?? Guid.NewGuid(),
            Name = generatedName.Length > WorkspaceName.MaxLength ? generatedName[..WorkspaceName.MaxLength] : generatedName,
            CreatedOn = FakerHub.Date.Past()
        };
    }
}