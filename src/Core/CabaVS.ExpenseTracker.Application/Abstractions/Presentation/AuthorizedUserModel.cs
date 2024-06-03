namespace CabaVS.ExpenseTracker.Application.Abstractions.Presentation;

public sealed record AuthorizedUserModel(Guid Id, string FullName, bool IsAdmin);