namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

public sealed record WorkspaceDetailsModel(Guid Id, string Name, WorkspaceMemberModel[] Members);
