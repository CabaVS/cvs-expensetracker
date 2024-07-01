using Bogus;
using Bogus.Extensions;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.FakeData;

internal sealed class IncomeTransactionFaker(Guid sourceId, Guid destinationId) : Faker<IncomeTransaction>
{
    public override IncomeTransaction Generate(string? ruleSets = null)
    {
        var amount = FakerHub.Finance.Amount();
        
        return new IncomeTransaction
        {
            Id = Guid.NewGuid(),
            Date = FakerHub.Date.PastDateOnly(),
            AmountInDestinationCurrency = amount,
            DestinationId = destinationId,
            AmountInSourceCurrency = amount,
            SourceId = sourceId,
            Tags = FakerHub.Random
                .WordsArray(2)
                .Select(x => x.ClampLength(max: TransactionTag.MaxLength))
                .ToArray()
        };
    }
}