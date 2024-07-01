using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using CabaVS.ExpenseTracker.IntegrationTests.Common;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
using CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace CabaVS.ExpenseTracker.IntegrationTests.Endpoints;

public sealed class WorkspaceEndpointsIntegrationTest(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
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
        const string url = "api/workspaces";
        
        var request = new CreateWorkspaceEndpoint.RequestModel(
            "My Workspace");
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
            .Where(uw => uw.UserId == CurrentUserAccessorInjected.AuthorizedUser.Id)
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
        _createdWorkspaceId = createdWorkspaces.Single().Id;
    }

    [Fact, TestOrder(Order = 3)]
    public async Task Update_ShouldBe_Successful_WhenModelIsValid()
    {
        // Ensure shared state
        if (_createdWorkspaceId is not { } workspaceId)
        {
            throw new InvalidOperationException(
                "Shared state is not valid. Unable to find previously created Workspace Id.");
        }
        
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);

        // Prepare request data
        var url = $"api/workspaces/{workspaceId}";
        
        var request = new UpdateWorkspaceEndpoint.RequestModel(
            Guid.Empty,
            "My Workspace UPD.");
        var content = ToJsonContent(request);
        
        // Assert database state (before request)
        var workspace = await FetchSingleWorkspace();
        workspace.Name.Should().NotBe(request.Name);
        
        // Execute request
        var endpointResponse = await Client.PutAsync(url, content); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await endpointResponse.Content.ReadAsStringAsync();
        response.Should().BeEmpty();
        
        // Assert database state (after request)
        workspace = await FetchSingleWorkspace();
        workspace.Name.Should().Be(request.Name);
        
        return;

        Task<Workspace> FetchSingleWorkspace() =>
            dbContext.Workspaces
                .AsNoTracking()
                .Where(w => w.Id == workspaceId)
                .SingleAsync();
    }

    [Fact, TestOrder(Order = 4)]
    public async Task Delete_ShouldBe_Successful_UserIsAdminOnThatWorkspace()
    {
        // Ensure shared state
        if (_createdWorkspaceId is not { } workspaceId)
        {
            throw new InvalidOperationException(
                "Shared state is not valid. Unable to find previously created Workspace Id.");
        }
        
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Prepare request data
        var url = $"api/workspaces/{workspaceId}";
        
        // Assert database state (before request)
        var userWorkspaceExists = await UserWorkspaceExists();
        userWorkspaceExists.Should().BeTrue();
        
        // Execute request
        var endpointResponse = await Client.DeleteAsync(url); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = await endpointResponse.Content.ReadAsStringAsync();
        response.Should().BeEmpty();
        
        // Assert database state (after request)
        userWorkspaceExists = await UserWorkspaceExists();
        userWorkspaceExists.Should().BeFalse();

        var workspaceExists = await dbContext.Workspaces.AnyAsync(w => w.Id == workspaceId);
        workspaceExists.Should().BeFalse();
        
        // Change shared state
        _createdWorkspaceId = null;
        
        return;

        Task<bool> UserWorkspaceExists() =>
            dbContext.UserWorkspaces
                .AsNoTracking()
                .Where(uw => uw.WorkspaceId == workspaceId)
                .Where(uw => uw.UserId == CurrentUserAccessorInjected.AuthorizedUser.Id)
                .Where(uw => uw.IsAdmin)
                .AnyAsync();
    }

    private static Guid? _createdWorkspaceId;

    // TODO: Get All should return multiple Workspaces if multiple exists for User
    // TODO: Get by Id should return a Workspace if User have access to it
    // TODO: Get by Id should return an error if Workspace doesn't exists
    // TODO: Get by Id should return an error if User doesn't belong to that Workspace
    // TODO: Create should return an error if model is not valid
    // TODO: Update should return an error if model is not valid
    // TODO: Update should return an error if Workspace doesn't exists
    // TODO: Update should return an error if User doesn't belong to that Workspace
    // TODO: Delete should return an error if Workspace doesn't exists
    // TODO: Delete should return an error if User doesn't belong to that Workspace
    // TODO: Delete should delete all children entities of that Workspace (B, EC, IC, ET, IT, TT)
}