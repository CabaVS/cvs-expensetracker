using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.IntegrationTests.Common;
using CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace CabaVS.ExpenseTracker.IntegrationTests.Endpoints;

public sealed class WorkspaceEndpointsIntegrationTest(IntegrationTestWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact, TestOrder(Order = 1)]
    public async Task GetAll_ShouldReturn_EmptyArray_WhenUserIsNotPartOfAnyWorkspace()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Prepare request data
        const string url = "api/workspaces";
        
        // Assert database state (before request)
        var numberOfWorkspacesInDatabase = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabase.Should().BeGreaterThan(0);
        
        // Execute request
        var endpointResponse = await Client.GetAsync(url); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel[]>();
        response.Should().BeEquivalentTo(Array.Empty<WorkspaceModel>());
    }
    
    [Fact, TestOrder(Order = 2)]
    public async Task Create_ShouldBe_Successful_WhenModelIsValid()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);

        // Prepare request data
        var request = new CreateWorkspaceEndpoint.RequestModel(
            "My Workspace");

        const string url = "api/workspaces";
        var content = ToJsonContent(request);
        
        // Assert database state (before request)
        var numberOfWorkspacesInDatabaseBeforeCreation = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabaseBeforeCreation.Should().BeGreaterThan(0);
        
        // Execute request
        var endpointResponse = await Client.PostAsync(url, content); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var response = await endpointResponse.Content.ReadAsStringAsync();
        response.Should().BeEmpty();

        var locationHeaderValues = endpointResponse.Headers.GetValues(HeaderNames.Location);
        locationHeaderValues.Should().HaveCount(1);
        
        // Assert database state (after request)
        var numberOfWorkspacesInDatabaseAfterCreation = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabaseAfterCreation.Should().Be(numberOfWorkspacesInDatabaseBeforeCreation + 1);

        var createdUserWorkspaces = await dbContext.UserWorkspaces
            .AsNoTracking()
            .Where(uw => uw.UserId == SharedState.Instance[StateKeys.AuthenticatedUser])
            .ToArrayAsync();
        createdUserWorkspaces.Should().HaveCount(1);

        var userWorkspace = createdUserWorkspaces.Single();
        userWorkspace.IsAdmin.Should().BeTrue();
        
        var createdWorkspaces = await dbContext.Workspaces
            .AsNoTracking()
            .Where(w => w.Id == userWorkspace.WorkspaceId)
            .ToArrayAsync();
        createdWorkspaces.Should().HaveCount(1);

        var workspace = createdWorkspaces.Single();
        workspace.Name.Should().Be(request.Name);
        
        // Change shared state
        SharedState.Instance.Add(
            StateKeys.WorkspaceCreatedThroughEndpoint,
            createdWorkspaces.Single().Id);
    }

    [Fact, TestOrder(Order = 3)]
    public async Task Update_ShouldBe_Successful_WhenModelIsValid()
    {
        // Ensure shared state
        if (!SharedState.Instance.TryGetValue(
                StateKeys.WorkspaceCreatedThroughEndpoint,
                out var workspaceId))
        {
            throw new InvalidOperationException(
                "Shared state is not valid. Unable to find previously created Workspace Id.");
        }
        
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);

        // Prepare request data
        var request = new UpdateWorkspaceEndpoint.RequestModel(
            Guid.Empty,
            "My Workspace UPD.");
        
        var url = $"api/workspaces/{workspaceId}";
        var content = ToJsonContent(request);
        
        // Assert database state (before request)
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .SingleAsync(w => w.Id == workspaceId);
        workspace.Name.Should().NotBe(request.Name);
        
        // Execute request
        var endpointResponse = await Client.PutAsync(url, content); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await endpointResponse.Content.ReadAsStringAsync();
        response.Should().BeEmpty();
        
        // Assert database state (after request)
        workspace = await dbContext.Workspaces
            .AsNoTracking()
            .SingleAsync(w => w.Id == workspaceId);
        workspace.Name.Should().Be(request.Name);
    }
}