using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class UserQueryRepository(ApplicationDbContext dbContext) : IUserQueryRepository
{
    public async Task<bool> IsExistAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var isExist = await dbContext.Users
            .Where(x => x.Id == userId)
            .AnyAsync(cancellationToken);
        return isExist;
    }
}
