using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Shared;

public sealed class ErrorTests
{
    [Fact]
    public void Error_Should_ContainAllNestedErrors_WhenProvidedInConstructor()
    {
        // Arrange
        Error[] nestedErrors =
        [
            StringErrors.IsNullOrWhiteSpace("MyEntity", "Field1"),
            StringErrors.IsNullOrWhiteSpace("MyEntity", "Field2")
        ];
        
        // Act
        var parentError = new Error("ValidationFailure", "One or more validation errors occurred.", nestedErrors);
        
        // Assert
        Assert.Equivalent(nestedErrors, parentError.InnerErrors);
    }
}
