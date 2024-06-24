namespace CabaVS.ExpenseTracker.IntegrationTests.Common;

public static class SharedState
{
    public static readonly Dictionary<string, Guid> Instance = new();
}

public static class StateKeys
{
    public const string AuthenticatedUser = nameof(AuthenticatedUser);
    public const string User1 = nameof(User1);
    public const string User2 = nameof(User2);
    public const string User3 = nameof(User3);
    public const string User4 = nameof(User4);
    public const string Workspace1 = nameof(Workspace1);
    public const string Workspace2 = nameof(Workspace2);
    public const string Workspace3 = nameof(Workspace3);
    public const string Workspace4 = nameof(Workspace4);
    public const string Workspace5 = nameof(Workspace5);
    public const string Workspace6 = nameof(Workspace6);
    public const string Workspace7 = nameof(Workspace7);
    public const string Workspace8 = nameof(Workspace8);
    public const string WorkspaceCreatedThroughEndpoint = nameof(WorkspaceCreatedThroughEndpoint);
}