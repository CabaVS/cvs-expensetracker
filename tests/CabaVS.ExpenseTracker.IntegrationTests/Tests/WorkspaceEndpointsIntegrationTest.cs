using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
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
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.OK, "Expected OK status code to be returned");

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel[]>();
        response.Should().BeEquivalentTo(Array.Empty<WorkspaceModel>(), $"Expected empty array of {nameof(WorkspaceModel)}.");
    }
    
    [Fact, TestOrder(Order = 2)]
    public async Task GetById_ShouldReturn_NotFound_WhenWorkspaceDoesNotExist()
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
        response.Should().Be(WorkspaceErrors.NotFoundById(workspaceId), "Expected NotFound error to be returned.");
    }
    
    [Fact, TestOrder(Order = 3)]
    public async Task GetById_ShouldReturn_NotFound_WhenUserDoesNotHaveAccessToWorkspace()
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
        response.Should().Be(WorkspaceErrors.NotFoundById(workspaceId), "Expected NotFound error to be returned.");
    }
}