using Bogus;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class UserFaker(Guid? id = null) : Faker<User>
{
    public override User Generate(string? ruleSets = null)
    {
        return new User { Id = id ?? Guid.NewGuid() };
    }
}