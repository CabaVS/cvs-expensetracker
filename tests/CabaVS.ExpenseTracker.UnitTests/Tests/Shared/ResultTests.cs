using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Shared;

public sealed class ResultTests
{
    [Fact]
    public static void ResultWithoutResponse_Should_BeCreatedAsSuccess_WhenNoErrorsProvided()
    {
        // Act
        var result = Result.Success();
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ResultWithoutResponse_Should_BeCreatedAsFailure_WhenErrorProvided()
    {
        // Arrange
        Error error = StringErrors.IsTooShort("MyEntity", "Property", 2, "XYZ");
        
        // Act
        var result = Result.Failure(error);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }
    
    [Fact]
    public void ResultWithoutResponse_Should_ThrowArgumentException_WhenErrorProvidedForSuccessfulResult()
    {
        // Arrange
        Error error = StringErrors.IsTooShort("MyEntity", "Property", 2, "XYZ");
        
        // Act
        var action = new Action(() => _ = new DummyResult(true, error));
        
        // Assert
        Assert.ThrowsAny<ArgumentException>(action);
    }
    
    [Fact]
    public void ResultWithoutResponse_Should_ThrowInvalidOperationException_WhenErrorIsAccessedOnSuccessfulResult()
    {
        // Arrange
        var result = Result.Success();
        
        // Act
        var action = new Action(() => _ = result.Error);
        
        // Assert
        Assert.ThrowsAny<InvalidOperationException>(action);
    }
    
    [Fact]
    public void Result_Should_ThrowInvalidOperationException_WhenValueIsAccessedOnFailedResult()
    {
        // Arrange
        Error error = StringErrors.IsTooShort("MyEntity", "Property", 2, "XYZ");
        var result = Result<string>.Failure(error);
        
        // Act
        var action = new Action(() => _ = result.Value);
        
        // Assert
        Assert.ThrowsAny<InvalidOperationException>(action);
    }
    
    [Fact]
    public void Result_Should_BeCreatedAsFailure_WhenErrorProvided()
    {
        // Arrange
        Error error = StringErrors.IsTooShort("MyEntity", "Property", 2, "XYZ");
        
        // Act
        var result = Result<string>.Failure(error);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }
    
    private sealed class DummyResult(bool isSuccess, Error error) : Result(isSuccess, error);
}
