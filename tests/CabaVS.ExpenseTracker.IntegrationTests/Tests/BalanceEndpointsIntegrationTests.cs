using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.IntegrationTests.Fakers;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
using CabaVS.ExpenseTracker.IntegrationTests.Seeders;
using CabaVS.ExpenseTracker.Persistence.Entities;
using CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.IntegrationTests.Tests;

public sealed class BalanceEndpointsIntegrationTests : IntegrationTestBase
{
    private readonly Guid _workspaceId;
    private Guid _balanceId;
    
    public BalanceEndpointsIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        DefaultDatabaseSeeder.SeedWorkspaces(
            DbContext, CurrentUserAccessorInjected.AuthenticatedUserId, 
            isAdmin: false, numberOfWorkspaces: 2);
        
        Guid[] generatedWorkspaces = DbContext.UserWorkspaces
            .Where(uw => uw.UserId == CurrentUserAccessorInjected.AuthenticatedUserId)
            .Select(w => w.WorkspaceId)
            .ToArray();
        Guid currencyId = DbContext.Currencies
            .Select(c => c.Id)
            .First();
        
        DefaultDatabaseSeeder.SeedBalances(DbContext, currencyId, generatedWorkspaces.First(), 2);
        
        _workspaceId = generatedWorkspaces.Last();
    }
    
    [Fact, TestOrder(Order = 1)]
    public async Task GetAll_ShouldReturn_EmptyCollection_WhenNoBalancesUnderWorkspace()
    {
        // Arrange
        var url = new Uri($"api/workspaces/{_workspaceId}/balances", UriKind.Relative);
        
        // Act
        HttpResponseMessage endpointResponse = await Client.GetAsync(url);
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.OK);

        BalanceModel[]? response = await endpointResponse.Content.ReadFromJsonAsync<BalanceModel[]>();
        response.Should().BeEmpty();
    }
    
    [Fact, TestOrder(Order = 2)]
    public async Task Create_Should_CreateBalance_WhenRequestIsValid()
    {
        // Arrange
        Guid currencyId = await DbContext.Currencies
            .Select(c => c.Id)
            .FirstAsync();
        
        Balance fakeBalance = new BalanceFaker(currencyId, _workspaceId).Generate();
        
        var url = new Uri($"api/workspaces/{_workspaceId}/balances", UriKind.Relative);
        var payload = new CreateBalanceEndpoint.RequestModel(
            fakeBalance.Name, fakeBalance.Amount, currencyId, _workspaceId);
        
        // Act
        HttpResponseMessage endpointResponse = await Client.PostAsJsonAsync(url, payload);
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.Created);
        
        Uri locationHeader = endpointResponse.Headers.Location!;
        locationHeader.Should().NotBeNull();

        _balanceId = Guid.Parse(locationHeader.Segments.Last());
        Balance balanceInDb = await DbContext.Balances
            .AsNoTracking()
            .Where(b => b.Id == _balanceId)
            .SingleAsync();
        
        balanceInDb.Id.Should().Be(_balanceId);
        balanceInDb.CreatedOn.Should().Be(balanceInDb.ModifiedOn).And.NotBe(default);
        balanceInDb.Name.Should().Be(payload.Name);
        balanceInDb.Amount.Should().Be(payload.Amount);
        balanceInDb.CurrencyId.Should().Be(payload.CurrencyId);
        balanceInDb.WorkspaceId.Should().Be(payload.WorkspaceId);
    }
}
