using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.IntegrationTests.Tests;

public sealed class CurrencyEndpointsIntegrationTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact, TestOrder(Order = 1)]
    public async Task GetAll_ShouldReturn_AllCurrencies()
    {
        // Arrange
        var url = new Uri("api/currencies", UriKind.Relative);

        CurrencyModel[] expectedCurrencyModels = await DbContext.Currencies
            .AsNoTracking()
            .Select(x => new CurrencyModel(
                x.Id, x.Name, x.Code, x.Symbol))
            .ToArrayAsync();
        
        // Act
        HttpResponseMessage endpointResponse = await Client.GetAsync(url);
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.OK);

        CurrencyModel[]? response = await endpointResponse.Content.ReadFromJsonAsync<CurrencyModel[]>();
        response.Should().HaveCountGreaterThan(0).And.BeEquivalentTo(expectedCurrencyModels);
    }
}
