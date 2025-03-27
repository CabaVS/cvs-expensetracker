namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

public sealed record WorkspaceMemberModel(Guid Id, bool IsAdmin, Guid UserId, string UserName);
