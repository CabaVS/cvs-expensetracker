using CabaVS.ExpenseTracker.Domain.Primitives;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Primitives;

public sealed class AuditableEntityTests
{
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithOperator_WhenBothAreNull()
    {
        // Arrange
        EntityOne? one = null;
        EntityOne? two = null;
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithOperator_WhenFirstIsNull()
    {
        // Arrange
        EntityOne? one = null;
        var two = new EntityOne(Guid.NewGuid(), default, default);
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithOperator_WhenSecondIsNull()
    {
        // Arrange
        var one = new EntityOne(Guid.NewGuid(), default, default);
        EntityOne? two = null;
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithOperator_WhenDifferentTypes()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        var two = new EntityTwo(id, default, default);
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_Should_BeEqualWithOperator_WhenSameTypesAndSameIds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        var two = new EntityOne(id, default, default);
        
        // Act
        var result = one == two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithEqualsMethod_WhenSecondIsNull()
    {
        // Arrange
        var one = new EntityOne(Guid.NewGuid(), default, default);
        EntityOne? two = null;
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithEqualsMethod_WhenDifferentTypes()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        var two = new EntityTwo(id, default, default);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AuditableEntities_Should_BeEqualWithEqualsMethod_WhenSameTypesAndSameIds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        var two = new EntityOne(id, default, default);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithEqualsMethod_WhenSecondIsNull_ButSecondIsBoxed()
    {
        // Arrange
        var one = new EntityOne(Guid.NewGuid(), default, default);
        object? two = null;
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AuditableEntities_ShouldNot_BeEqualWithEqualsMethod_WhenDifferentTypes_ButSecondIsBoxed()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        object two = new EntityTwo(id, default, default);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AuditableEntities_Should_BeEqualWithEqualsMethod_WhenSameTypesAndSameIds_ButSecondIsBoxed()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        object two = new EntityOne(id, default, default);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_Should_ReturnDifferentHashcode_WhenSameTypesButDifferentIds()
    {
        // Arrange
        var one = new EntityOne(Guid.NewGuid(), default, default);
        var two = new EntityOne(Guid.NewGuid(), default, default);
        
        // Act
        var result = one.GetHashCode() != two.GetHashCode();
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_Should_ReturnDifferentHashcode_WhenDifferentTypesButSameIds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        var two = new EntityTwo(id, default, default);
        
        // Act
        var result = one.GetHashCode() != two.GetHashCode();
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AuditableEntities_Should_ReturnSameHashcode_WhenSameTypesAndSameIds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var one = new EntityOne(id, default, default);
        var two = new EntityOne(id, default, default);
        
        // Act
        var result = one.GetHashCode() == two.GetHashCode();
        
        // Assert
        Assert.True(result);
    }
    
    private sealed class EntityOne(Guid id, DateTime createdOn, DateTime modifiedOn) : AuditableEntity(id, createdOn, modifiedOn);
    private sealed class EntityTwo(Guid id, DateTime createdOn, DateTime modifiedOn) : AuditableEntity(id, createdOn, modifiedOn);
}