using System.Net;
using System.Net.Http.Json;
using Bogus;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.IntegrationTests.Tests;

public sealed class WorkspaceEndpointsIntegrationTest(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    private static readonly List<WorkspaceModel> ExpectedWorkspaces = [];
    
    [Fact, TestOrder(Order = 1)]
    public async Task GetAll_ShouldReturn_EmptyCollection_WhenUserIsNotPartOfAnyWorkspace()
    {
        // Arrange
        const string url = "api/workspaces";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        var numberOfWorkspacesInDatabase = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabase.Should().BeGreaterThan(0, "Number of workspaces should be greater than 0.");
        
        // Act
        var endpointResponse = await Client.GetAsync(url);
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.OK, "Expected OK status code to be returned");

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel[]>();
        response.Should().BeEquivalentTo(Array.Empty<WorkspaceModel>(), $"Expected an empty collection of {nameof(WorkspaceModel)}.");
    }
    
    [Fact, TestOrder(Order = 2)]
    public async Task GetById_ShouldReturnError_NotFoundById_WhenWorkspaceDoesNotExist()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var url = $"api/workspaces/{workspaceId}";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        var workspaceByGeneratedIdExist = await dbContext.Workspaces.AnyAsync(w => w.Id == workspaceId);
        workspaceByGeneratedIdExist.Should().BeFalse();
        
        // Act
        var endpointResponse = await Client.GetAsync(url);
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest, "Expected BadRequest status code to be returned.");

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceErrors.NotFoundById(workspaceId), $"Expected {nameof(WorkspaceErrors.NotFoundById)} error to be returned.");
    }
    
    [Fact, TestOrder(Order = 3)]
    public async Task GetById_ShouldReturnError_NotFoundById_WhenUserDoesNotHaveAccessToWorkspace()
    {
        // Arrange
        var workspaceId = await ConvertTo<ApplicationDbContext>(DbContext)
            .UserWorkspaces
            .Where(uw => uw.UserId != CurrentUserAccessorInjected.AuthenticatedUserId)
            .Select(uw => uw.WorkspaceId)
            .FirstAsync();
        var url = $"api/workspaces/{workspaceId}";
        
        // Act
        var endpointResponse = await Client.GetAsync(url);
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest, "Expected BadRequest status code to be returned.");

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceErrors.NotFoundById(workspaceId), $"Expected {nameof(WorkspaceErrors.NotFoundById)} error to be returned.");
    }
    
    [Fact, TestOrder(Order = 4)]
    public async Task CreateWorkspace_ShouldReturnError_NameIsNullOrWhitespace_WhenWorkspaceNameIsEmpty()
    {
        // Arrange
        const string workspaceName = "";
        const string url = "api/workspaces";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        var numberOfWorkspacesInDatabaseBeforeRequest = await dbContext.Workspaces.CountAsync();
        
        // Act
        var endpointResponse = await Client.PostAsJsonAsync(url, new CreateWorkspaceEndpoint.RequestModel(workspaceName));
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest, "Expected BadRequest status code to be returned.");

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceErrors.NameIsNullOrWhitespace(), $"Expected {nameof(WorkspaceErrors.NameIsNullOrWhitespace)} error to be returned.");
        
        var numberOfWorkspacesInDatabaseAfterRequest = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabaseBeforeRequest.Should().Be(numberOfWorkspacesInDatabaseAfterRequest, "Number of workspaces should not change.");
    }
    
    [Fact, TestOrder(Order = 5)]
    public async Task CreateWorkspace_ShouldReturnError_NameTooLong_WhenWorkspaceNameIsTooLong()
    {
        // Arrange
        var workspaceName = new Faker().Random.String2(WorkspaceName.MaxLength + 1);
        const string url = "api/workspaces";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        var numberOfWorkspacesInDatabaseBeforeRequest = await dbContext.Workspaces.CountAsync();
        
        // Act
        var endpointResponse = await Client.PostAsJsonAsync(url, new CreateWorkspaceEndpoint.RequestModel(workspaceName));
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest, "Expected BadRequest status code to be returned.");

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceErrors.NameTooLong(workspaceName), $"Expected {nameof(WorkspaceErrors.NameTooLong)} error to be returned.");
        
        var numberOfWorkspacesInDatabaseAfterRequest = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabaseBeforeRequest.Should().Be(numberOfWorkspacesInDatabaseAfterRequest, "Number of workspaces should not change.");
    }
    
    [Fact, TestOrder(Order = 6)]
    public async Task CreateWorkspace_ShouldReturn_LocationInHeader_WhenRequestIsValid()
    {
        // Arrange
        var workspaceName = new Faker().Random.String2(1, WorkspaceName.MaxLength);
        const string url = "api/workspaces";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        var numberOfWorkspacesInDatabaseBeforeRequest = await dbContext.Workspaces.CountAsync();
        
        // Act
        var endpointResponse = await Client.PostAsJsonAsync(url, new CreateWorkspaceEndpoint.RequestModel(workspaceName));
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.Created, "Expected Created status code to be returned.");

        var responseLength = endpointResponse.Content.Headers.ContentLength;
        responseLength.Should().Be(0, "Expected an empty content to be returned.");
        
        var numberOfWorkspacesInDatabaseAfterRequest = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabaseBeforeRequest.Should().Be(numberOfWorkspacesInDatabaseAfterRequest - 1, "Number of workspaces should change by 1.");
        
        var locationHeader = endpointResponse.Headers.Location?.ToString();
        locationHeader.Should().NotBeNullOrWhiteSpace().And.Contain("api/workspaces/");
        
        var workspaceIdFromLocationHeader = Guid.Parse(
            locationHeader!
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Last());
        var workspaceIdFromDatabase = await dbContext.Workspaces
            .Where(w => w.Id == workspaceIdFromLocationHeader)
            .Select(w => w.Id)
            .FirstOrDefaultAsync();
        workspaceIdFromDatabase.Should().Be(workspaceIdFromLocationHeader, "Expected workspace with ID from Location header to exist in Database.");
        
        ExpectedWorkspaces.Add(new WorkspaceModel(workspaceIdFromLocationHeader, workspaceName, true));
    }
    
    [Fact, TestOrder(Order = 7)]
    public async Task UpdateWorkspace_ShouldReturnError_NameIsNullOrWhitespace_WhenWorkspaceNameIsEmpty()
    {
        // Arrange
        const string workspaceName = "";
        var url = $"api/workspaces/{ExpectedWorkspaces.Single().Id}";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Act
        var endpointResponse = await Client.PutAsJsonAsync(url, new UpdateWorkspaceEndpoint.RequestModel(default, workspaceName));
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest, "Expected BadRequest status code to be returned.");

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceErrors.NameIsNullOrWhitespace(), $"Expected {nameof(WorkspaceErrors.NameIsNullOrWhitespace)} error to be returned.");
        
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .SingleAsync(w => w.Id == ExpectedWorkspaces.Single().Id);
        workspace.Name.Should().Be(ExpectedWorkspaces.Single().Name).And.NotBe(workspaceName);
    }
    
    [Fact, TestOrder(Order = 8)]
    public async Task UpdateWorkspace_ShouldReturnError_NameTooLong_WhenWorkspaceNameIsTooLong()
    {
        // Arrange
        var workspaceName = new Faker().Random.String2(WorkspaceName.MaxLength + 1);
        var url = $"api/workspaces/{ExpectedWorkspaces.Single().Id}";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Act
        var endpointResponse = await Client.PutAsJsonAsync(url, new UpdateWorkspaceEndpoint.RequestModel(default, workspaceName));
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest, "Expected BadRequest status code to be returned.");

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceErrors.NameTooLong(workspaceName), $"Expected {nameof(WorkspaceErrors.NameTooLong)} error to be returned.");
        
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .SingleAsync(w => w.Id == ExpectedWorkspaces.Single().Id);
        workspace.Name.Should().Be(ExpectedWorkspaces.Single().Name).And.NotBe(workspaceName);
    }
    
    [Fact, TestOrder(Order = 9)]
    public async Task UpdateWorkspace_ShouldReturn_OK_WhenRequestIsValid()
    {
        // Arrange
        var workspaceName = new Faker().Random.String2(1, WorkspaceName.MaxLength);
        var url = $"api/workspaces/{ExpectedWorkspaces.Single().Id}";
        
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Act
        var endpointResponse = await Client.PutAsJsonAsync(url, new CreateWorkspaceEndpoint.RequestModel(workspaceName));
        
        // Assert
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.OK, "Expected OK status code to be returned.");

        var responseLength = endpointResponse.Content.Headers.ContentLength;
        responseLength.Should().Be(0, "Expected an empty content to be returned.");
        
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .SingleAsync(w => w.Id == ExpectedWorkspaces.Single().Id);
        workspace.Name.Should().Be(workspaceName);
        
        ExpectedWorkspaces[0] = ExpectedWorkspaces.Single() with { Name = workspaceName };
    }
}