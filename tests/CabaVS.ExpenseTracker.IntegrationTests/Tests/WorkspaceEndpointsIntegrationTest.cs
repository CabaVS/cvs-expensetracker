using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.IntegrationTests.Tests;

public sealed class WorkspaceEndpointsIntegrationTest(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact, TestOrder(Order = 1)]
    public async Task GetAll_ShouldReturn_EmptyArray_WhenUserIsNotPartOfAnyWorkspace()
    {
        // Arrange
        const string url = "api/workspaces";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        var numberOfWorkspacesInDatabase = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabase.Should().BeGreaterThan(0, "Number of workspaces should be greater than 0.");
        
        // Act
        var endpointResponse = await Client.GetAsync(url); 
        
        // Assert
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel[]>();
        response.Should().BeEquivalentTo(Array.Empty<WorkspaceModel>(), $"Expected empty array of {nameof(WorkspaceModel)}.");
    }
}