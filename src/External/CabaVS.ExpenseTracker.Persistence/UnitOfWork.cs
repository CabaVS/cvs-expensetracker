﻿using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Repositories;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);

    private readonly Lazy<IUserRepository> _userRepository = new(() => new UserRepository(dbContext));
    public IUserRepository UserRepository => _userRepository.Value;
    
    private readonly Lazy<IWorkspaceRepository> _workspaceRepository = new(() => new WorkspaceRepository(dbContext));
    public IWorkspaceRepository WorkspaceRepository => _workspaceRepository.Value;
}
