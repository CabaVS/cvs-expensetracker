namespace CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;

public sealed record AuthenticatedUserModel(Guid Id, bool IsAdmin);