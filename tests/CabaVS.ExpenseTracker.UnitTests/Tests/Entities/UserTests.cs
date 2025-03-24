using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Entities;

public sealed class UserTests
{
    [Fact]
    public void User_CreateNew_Should_CreateUser_And_GenerateIdAndDates_WhenInputIsValid()
    {
        // Arrange
        DateTime timeStampBefore = DateTime.UtcNow;
        const string username = "username";
        const bool isAdmin = false;
        
        // Act
        Result<User> result = User.CreateNew(username, isAdmin);
        
        // Assert
        DateTime timeStampAfter = DateTime.UtcNow;
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.InRange(result.Value.CreatedOn, timeStampBefore, timeStampAfter);
        Assert.InRange(result.Value.ModifiedOn, timeStampBefore, timeStampAfter);
        Assert.Equal(UserName.Create(username).Value, result.Value.UserName);
        Assert.Equal(isAdmin, result.Value.IsAdmin);
    }
    
    [Fact]
    public void User_CreateExisting_Should_CreateUser_And_HonorIdAndDates_WhenInputIsValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        DateTime timeStamp = DateTime.UtcNow;
        const string username = "username";
        const bool isAdmin = false;
        
        // Act
        Result<User> result = User.CreateExisting(id, timeStamp, timeStamp, username, isAdmin);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(timeStamp, result.Value.CreatedOn);
        Assert.Equal(timeStamp, result.Value.ModifiedOn);
        Assert.Equal(UserName.Create(username).Value, result.Value.UserName);
        Assert.Equal(isAdmin, result.Value.IsAdmin);
    }
}
