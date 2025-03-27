﻿using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;

namespace CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;

public interface IReadOnlyWorkspaceRepository
{
    Task<WorkspaceCollectionItemModel[]> GetAllAsync(Guid userId, CancellationToken cancellationToken);
}
