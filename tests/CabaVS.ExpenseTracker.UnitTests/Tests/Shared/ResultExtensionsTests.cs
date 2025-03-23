using CabaVS.ExpenseTracker.Domain.Shared;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Shared;

public sealed class ResultExtensionsTests
{
    [Fact]
    public void Ensure_Should_PropagateResultFurther_WhenItsAlreadyFailed()
    {
        // Arrange
        var error = new Error("Error", "Failure");
        
        // Act
        Result<string> result = Result<string>.Success(string.Empty)
            .Ensure(_ => false, error)
            .Ensure(_ => true, error);
        
        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void Ensure_Should_ReturnSuccessfulResult_WhenPredicatePasses()
    {
        // Act
        Result<string> result = Result<string>.Success(string.Empty)
            .Ensure(_ => true, new Error("Error", "Failure"));
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void Ensure_Should_ReturnFailedResult_WhenPredicateFails()
    {
        // Act
        Result<string> result = Result<string>.Success(string.Empty)
            .Ensure(_ => false, new Error("Error", "Failure"));
        
        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void EnsureStringNotTooShort_Should_ReturnSuccessfulResult_WhenStringIsNotTooShort()
    {
        // Arrange
        const string value = "123";
        
        // Act
        Result<string> result = Result<string>.Success(value)
            .EnsureStringNotTooShort(2, new Error("Error", "TooShort"));
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void EnsureStringNotTooShort_Should_ReturnFailedResult_WhenStringIsTooShort()
    {
        // Arrange
        const string value = "123";
        
        // Act
        Result<string> result = Result<string>.Success(value)
            .EnsureStringNotTooShort(4, new Error("Error", "TooShort"));
        
        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void Map_Should_PropagateResultFurther_WhenItsAlreadyFailed()
    {
        // Arrange
        var error = new Error("Error", "Failure");
        
        // Act
        Result<object> result = Result<string>.Success(string.Empty)
            .Ensure(_ => false, error)
            .Map(object (value) => value);
        
        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void Map_Should_PropagateResultFurther_WhenItsAlreadyFailed_And_WrappedInResult()
    {
        // Arrange
        var error = new Error("Error", "Failure");
        
        // Act
        Result<object> result = Result<string>.Success(string.Empty)
            .Ensure(_ => false, error)
            .Map(Result<object>.Success);
        
        // Assert
        Assert.True(result.IsFailure);
    }
}
