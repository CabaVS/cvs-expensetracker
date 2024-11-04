using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class UserReadRepository(ApplicationDbContext dbContext) : IUserReadRepository
{
    public async Task<bool> IsExistById(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    }
}