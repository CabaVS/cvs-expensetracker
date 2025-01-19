using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<Guid> Create(Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        var userToCreate = User.ConvertFromDomainEntity(user);
        
        EntityEntry<User> entityEntry = await dbContext.Users.AddAsync(userToCreate, cancellationToken);
        
        return entityEntry.Entity.Id;
    }
}
