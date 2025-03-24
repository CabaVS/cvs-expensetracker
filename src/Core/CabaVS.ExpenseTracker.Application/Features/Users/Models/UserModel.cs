namespace CabaVS.ExpenseTracker.Application.Features.Users.Models;

public sealed record UserModel(Guid Id, string UserName, bool IsAdmin);
