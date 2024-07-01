using Bogus;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class AuthorizedUserModelFaker(bool isAdmin = true) : Faker<AuthorizedUserModel>
{
    public override AuthorizedUserModel Generate(string? ruleSets = null)
    {
        return new AuthorizedUserModel(
            Guid.NewGuid(),
            FakerHub.Internet.UserName(),
            isAdmin);
    }
}