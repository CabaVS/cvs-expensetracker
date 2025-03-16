namespace CabaVS.ExpenseTracker.Domain.Shared;

public sealed record Error(string Code, string Description)
{
    public Error[] InnerErrors { get; init; } = [];

    public Error(string code, string description, Error[] innerErrors) : this(code, description)
    {
        InnerErrors = innerErrors;
    }
    
    public static readonly Error None = new(string.Empty, string.Empty);
}