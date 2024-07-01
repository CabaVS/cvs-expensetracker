using System.Net;
using System.Net.Http.Json;
using CabaVS.ExpenseTracker.Application.Common.Errors;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using CabaVS.ExpenseTracker.IntegrationTests.Common;
using CabaVS.ExpenseTracker.IntegrationTests.FakeData;
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
    
    [Fact, TestOrder(Order = 5)]
    public async Task GetAll_ShouldReturn_Collection_WhenUserHaveMultipleWorkspaces()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Prepare request data
        const string url = "api/workspaces";
        
        // Change data directly through database
        var generatedWorkspaces = new WorkspaceFaker()
            .Generate(2)
            .Select((x, i) =>
            {
                x.UserWorkspaces =
                [
                    new UserWorkspace
                    {
                        WorkspaceId = x.Id,
                        UserId = CurrentUserAccessorInjected.AuthorizedUser.Id,
                        IsAdmin = i % 2 == 0
                    }
                ];
                return x;
            })
            .ToArray();
        
        await dbContext.Workspaces.AddRangeAsync(generatedWorkspaces);
        await dbContext.SaveChangesAsync();
        
        // Assert database state (before request)
        var numberOfWorkspacesInDatabase = await dbContext.Workspaces.CountAsync();
        numberOfWorkspacesInDatabase.Should().BeGreaterThan(generatedWorkspaces.Length);
        
        // Execute request
        var endpointResponse = await Client.GetAsync(url); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel[]>();
        response.Should().BeEquivalentTo(
            generatedWorkspaces.Select(
                x => new WorkspaceModel(x.Id, x.Name, x.UserWorkspaces.Single().IsAdmin)));
    }
    
    [Fact, TestOrder(Order = 6)]
    public async Task GetById_ShouldReturn_OK_WhenUserIsAdminOfWorkspace()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Ensure shared state
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .Where(w => w.UserWorkspaces
                .Any(uw => uw.UserId == CurrentUserAccessorInjected.AuthorizedUser.Id && uw.IsAdmin))
            .SingleOrDefaultAsync();
        if (workspace is null)
        {
            throw new InvalidOperationException(
                "Shared state is not valid. Unable to find previously created Workspace Id.");
        }
        
        // Prepare request data
        var url = $"api/workspaces/{workspace.Id}";
        
        // Execute request
        var endpointResponse = await Client.GetAsync(url); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel>();
        response.Should().Be(new WorkspaceModel(workspace.Id, workspace.Name, true));
    }
    
    [Fact, TestOrder(Order = 7)]
    public async Task GetById_ShouldReturn_OK_WhenUserIsNotAdminOfWorkspace()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Ensure shared state
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .Where(w => w.UserWorkspaces
                .Any(uw => uw.UserId == CurrentUserAccessorInjected.AuthorizedUser.Id && !uw.IsAdmin))
            .SingleOrDefaultAsync();
        if (workspace is null)
        {
            throw new InvalidOperationException(
                "Shared state is not valid. Unable to find previously created Workspace Id.");
        }
        
        // Prepare request data
        var url = $"api/workspaces/{workspace.Id}";
        
        // Execute request
        var endpointResponse = await Client.GetAsync(url); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await endpointResponse.Content.ReadFromJsonAsync<WorkspaceModel>();
        response.Should().Be(new WorkspaceModel(workspace.Id, workspace.Name, false));
    }
    
    [Fact, TestOrder(Order = 8)]
    public async Task GetById_ShouldReturn_Error_WhenUserDoesNotBelongToWorkspace()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Ensure shared state
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .Where(w => w.UserWorkspaces
                .All(uw => uw.UserId != CurrentUserAccessorInjected.AuthorizedUser.Id))
            .FirstOrDefaultAsync();
        if (workspace is null)
        {
            throw new InvalidOperationException(
                "Shared state is not valid. Unable to find previously created Workspace Id.");
        }
        
        // Prepare request data
        var url = $"api/workspaces/{workspace.Id}";
        
        // Execute request
        var endpointResponse = await Client.GetAsync(url); 
        
        // Assert response
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceAccessErrors.NoAccess(workspace.Id));
    }
    
    [Fact, TestOrder(Order = 9)]
    public async Task Delete_ShouldNot_BeSuccessful_WhenUserIsNotAdminOverWorkspace()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Assert database state (before request)
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .Include(w => w.UserWorkspaces)
            .Where(w => w.UserWorkspaces
                .Any(uw => uw.UserId == CurrentUserAccessorInjected.AuthorizedUser.Id && !uw.IsAdmin))
            .SingleOrDefaultAsync();
        if (workspace is null) throw new InvalidOperationException();
        
        // Prepare request data
        var url = $"api/workspaces/{workspace.Id}";
        
        // Execute request
        var endpointResponse = await Client.DeleteAsync(url); 
        
        // Assert response
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        
        var response = await endpointResponse.Content.ReadFromJsonAsync<Error>();
        response.Should().Be(WorkspaceAccessErrors.NotAdmin(workspace.Id));
        
        // Assert database state (after request)
        var userWorkspaceExists = await dbContext.UserWorkspaces
            .AsNoTracking()
            .Where(uw => uw.WorkspaceId == workspace.Id)
            .AnyAsync();
        userWorkspaceExists.Should().BeTrue();

        var workspaceExists = await dbContext.Workspaces.AnyAsync(w => w.Id == workspace.Id);
        workspaceExists.Should().BeTrue();
    }
    
    [Fact, TestOrder(Order = 10)]
    public async Task Delete_Should_BeSuccessful_And_RemoveAllChildrenItems_WhenUserIsAdminOverWorkspace()
    {
        // Get DbContext
        var dbContext = ConvertTo<ApplicationDbContext>(DbContext);
        
        // Assert database state (before request)
        var workspace = await dbContext.Workspaces
            .AsNoTracking()
            .Where(w => w.UserWorkspaces
                .Any(uw => uw.UserId == CurrentUserAccessorInjected.AuthorizedUser.Id && uw.IsAdmin))
            .SingleOrDefaultAsync();
        if (workspace is null) throw new InvalidOperationException();

        var currencyId = await dbContext.Currencies
            .Select(c => c.Id)
            .FirstOrDefaultAsync();
        if (currencyId == default) throw new InvalidOperationException();
        
        // Change data directly through database
        Balance balance = new BalanceFaker(workspace.Id, currencyId);
        await dbContext.Balances.AddAsync(balance);
        await dbContext.SaveChangesAsync();
        
        IncomeCategory incomeCategory = new IncomeCategoryFaker(workspace.Id, currencyId);
        await dbContext.IncomeCategories.AddAsync(incomeCategory);
        await dbContext.SaveChangesAsync();
        
        var incomeTransactions = new IncomeTransactionFaker(incomeCategory.Id, balance.Id).Generate(10);
        await dbContext.IncomeTransactions.AddRangeAsync(incomeTransactions);
        await dbContext.SaveChangesAsync();
        
        await AssertChildrenExistence(workspace.Id, true);
        
        // Prepare request data
        var url = $"api/workspaces/{workspace.Id}";
        
        // Execute request
        var endpointResponse = await Client.DeleteAsync(url); 
        
        // Assert response
        endpointResponse.Should().BeSuccessful();
        endpointResponse.Should().HaveStatusCode(HttpStatusCode.OK);
        
        var response = await endpointResponse.Content.ReadAsStringAsync();
        response.Should().BeEmpty();
        
        // Assert database state (after request)
        var userWorkspaceExists = await dbContext.UserWorkspaces
            .AsNoTracking()
            .Where(uw => uw.WorkspaceId == workspace.Id)
            .AnyAsync();
        userWorkspaceExists.Should().BeFalse();

        var workspaceExists = await dbContext.Workspaces.AnyAsync(w => w.Id == workspace.Id);
        workspaceExists.Should().BeFalse();

        await AssertChildrenExistence(workspace.Id, false);

        return;

        async Task AssertChildrenExistence(Guid workspaceId, bool shouldExist)
        {
            var balancesExists = await dbContext.Balances.AnyAsync(b => b.WorkspaceId == workspaceId);
            balancesExists.Should().Be(shouldExist);
        
            var incomeCategoriesExists = await dbContext.IncomeCategories.AnyAsync(ic => ic.WorkspaceId == workspaceId);
            incomeCategoriesExists.Should().Be(shouldExist);
        
            var incomeTransactionsExists = await dbContext.IncomeTransactions.AnyAsync(it =>
                it.Source.WorkspaceId == workspaceId || 
                it.Destination.WorkspaceId == workspaceId);
            incomeTransactionsExists.Should().Be(shouldExist);
        }
    }

    private static Guid? _createdWorkspaceId;
}