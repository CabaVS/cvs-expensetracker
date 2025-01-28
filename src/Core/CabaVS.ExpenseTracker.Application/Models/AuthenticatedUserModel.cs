namespace CabaVS.ExpenseTracker.Application.Models;

public record AuthenticatedUserModel(Guid Id, string Name, bool IsAdmin);
