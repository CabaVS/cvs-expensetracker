using Bogus;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Fakers;

internal sealed class UserFaker(Guid? id = null) : Faker<User>
{
    public override User Generate(string? ruleSets = null)
    {
        DateTime modifiedOn = FakerHub.Date.Past();
        DateTime createdOn = FakerHub.Date.Past(yearsToGoBack: 2, refDate: modifiedOn);
        
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            CreatedOn = createdOn,
            ModifiedOn = modifiedOn
        };
    }
}
