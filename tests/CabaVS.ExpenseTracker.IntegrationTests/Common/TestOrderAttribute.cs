namespace CabaVS.ExpenseTracker.IntegrationTests.Common;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestOrderAttribute : Attribute
{
    public int Order { get; init; } = int.MaxValue;
}