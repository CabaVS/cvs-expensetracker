using CabaVS.ExpenseTracker.Domain.Primitives;
using Xunit;

namespace CabaVS.ExpenseTracker.UnitTests.Tests.Primitives;

public sealed class ValueObjectTests
{
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithOperator_WhenBothAreNull()
    {
        // Arrange
        ValueObjectOne? one = null;
        ValueObjectOne? two = null;
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithOperator_WhenFirstIsNull()
    {
        // Arrange
        ValueObjectOne? one = null;
        var two = new ValueObjectOne(string.Empty);
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithOperator_WhenSecondIsNull()
    {
        // Arrange
        var one = new ValueObjectOne(string.Empty);
        ValueObjectOne? two = null;
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithOperator_WhenDifferentTypes()
    {
        // Arrange
        var one = new ValueObjectOne(string.Empty);
        var two = new ValueObjectTwo(string.Empty);
        
        // Act
        var result = one != two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_Should_BeEqualWithOperator_WhenSameTypesAndSameValues()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        var two = new ValueObjectOne(value);
        
        // Act
        var result = one == two;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithEqualsMethod_WhenSecondIsNull()
    {
        // Arrange
        var one = new ValueObjectOne(string.Empty);
        ValueObjectOne? two = null;
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithEqualsMethod_WhenDifferentTypes()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        var two = new ValueObjectTwo(value);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void ValueObjects_Should_BeEqualWithEqualsMethod_WhenSameTypesAndSameValues()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        var two = new ValueObjectOne(value);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithEqualsMethod_WhenSecondIsNull_ButSecondIsBoxed()
    {
        // Arrange
        var one = new ValueObjectOne(string.Empty);
        object? two = null;
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void ValueObjects_ShouldNot_BeEqualWithEqualsMethod_WhenDifferentTypes_ButSecondIsBoxed()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        object two = new ValueObjectTwo(value);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void ValueObjects_Should_BeEqualWithEqualsMethod_WhenSameTypesAndSameValues_ButSecondIsBoxed()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        object two = new ValueObjectOne(value);
        
        // Act
        var result = one.Equals(two);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_Should_ReturnDifferentHashcode_WhenSameTypesButDifferentValues()
    {
        // Arrange
        var one = new ValueObjectOne("example value 1");
        var two = new ValueObjectOne("example value 2");
        
        // Act
        var result = one.GetHashCode() != two.GetHashCode();
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_Should_ReturnDifferentHashcode_WhenDifferentTypesButSameValues()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        var two = new ValueObjectTwo(value);
        
        // Act
        var result = one.GetHashCode() != two.GetHashCode();
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void ValueObjects_Should_ReturnSameHashcode_WhenSameTypesAndSameValues()
    {
        // Arrange
        const string value = "example value";
        var one = new ValueObjectOne(value);
        var two = new ValueObjectOne(value);
        
        // Act
        var result = one.GetHashCode() == two.GetHashCode();
        
        // Assert
        Assert.True(result);
    }
    
    private sealed class ValueObjectOne(string value) : ValueObject
    {
        protected override IEnumerable<object> AtomicValues
        {
            get
            {
                yield return value;
            }
        }
    }

    private sealed class ValueObjectTwo(string value) : ValueObject
    {
        protected override IEnumerable<object> AtomicValues
        {
            get
            {
                yield return value;
            }
        }
    }
}
