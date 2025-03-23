using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Entities;

public sealed class WorkspaceTests
{
    [Fact]
    public void Workspace_CreateNew_Should_CreateWorkspace_And_GenerateIdAndDates_WhenInputIsValid()
    {
        // Arrange
        DateTime timeStampBefore = DateTime.UtcNow;
        const string workspaceName = "My Workspace";
        
        User owner = User.CreateNew("username").Value;
        
        // Act
        Result<Workspace> result = Workspace.CreateNew(workspaceName, owner);
        
        // Assert
        DateTime timeStampAfter = DateTime.UtcNow;
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.InRange(result.Value.CreatedOn, timeStampBefore, timeStampAfter);
        Assert.InRange(result.Value.ModifiedOn, timeStampBefore, timeStampAfter);
        Assert.Equal(WorkspaceName.Create(workspaceName).Value, result.Value.Name);
        Assert.Equivalent(new[] { owner }, result.Value.Members.Select(x => x.User));
        Assert.True(result.Value.Members.Single().IsAdmin);
    }
    
    [Fact]
    public void Workspace_CreateExisting_Should_CreateWorkspace_And_HonorIdAndDates_WhenInputIsValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        DateTime timeStamp = DateTime.UtcNow;
        const string workspaceName = "My Workspace";
        
        WorkspaceMember owner = WorkspaceMember.CreateNew(
            User.CreateNew("username").Value,
            true).Value;
        
        // Act
        Result<Workspace> result = Workspace.CreateExisting(id, timeStamp, timeStamp, workspaceName, [owner]);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(timeStamp, result.Value.CreatedOn);
        Assert.Equal(timeStamp, result.Value.ModifiedOn);
        Assert.Equal(WorkspaceName.Create(workspaceName).Value, result.Value.Name);
        Assert.Equivalent(new[] { owner }, result.Value.Members);
    }
}
