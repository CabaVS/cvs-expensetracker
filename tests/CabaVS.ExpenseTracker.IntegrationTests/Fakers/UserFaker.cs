using Bogus;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class UserFaker(Guid? id = null) : Faker<User>
{
    public override User Generate(string? ruleSets = null) => 
        new()
        {
            Id = id ?? Guid.NewGuid(),
            CreatedOn = FakerHub.Date.Past()
        };
}
