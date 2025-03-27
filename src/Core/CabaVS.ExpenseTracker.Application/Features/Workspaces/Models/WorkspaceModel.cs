namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

public sealed record WorkspaceModel(Guid Id, string Name, WorkspaceMemberModel[] Members);
