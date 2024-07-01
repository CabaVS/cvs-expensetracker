using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using CabaVS.ExpenseTracker.IntegrationTests.FakeData;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;

namespace CabaVS.ExpenseTracker.IntegrationTests.DatabaseSeeders;

internal static class DefaultSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        using var transaction = dbContext.Database.BeginTransaction();

        SeedUsers();
        SeedWorkspaces();
        BindUsersToWorkspaces();
        
        transaction.Commit();
        return;

        void SeedUsers()
        {
            var users = new UserFaker().Generate(4);
            users.Add(new UserFaker(CurrentUserAccessorInjected.AuthorizedUser.Id));
            
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }

        void SeedWorkspaces()
        {
            var workspaces = new WorkspaceFaker().Generate(8);
            
            dbContext.Workspaces.AddRange(workspaces);
            dbContext.SaveChanges();
        }

        void BindUsersToWorkspaces()
        {
            var usersToBind = dbContext.Users
                .Where(u => u.Id != CurrentUserAccessorInjected.AuthorizedUser.Id)
                .Select(u => u.Id)
                .ToArray();
            var workspacesToBind = dbContext.Workspaces
                .Select(w => w.Id)
                .ToArray();

            if (usersToBind.Length == 0 || workspacesToBind.Length == 0)
                throw new InvalidOperationException("Users or Workspaces not found.");
            if (workspacesToBind.Length / usersToBind.Length != 2)
                throw new InvalidOperationException("Number of Workspaces should be twice as number of Users.");

            var userWorkspaces = usersToBind
                .Select((u, i) =>
                    new UserWorkspace[]
                    {
                        new() { UserId = u, WorkspaceId = workspacesToBind[i], IsAdmin = true },
                        new() { UserId = u, WorkspaceId = workspacesToBind[i + usersToBind.Length], IsAdmin = false }
                    })
                .SelectMany(x => x)
                .ToArray();
            
            dbContext.UserWorkspaces.AddRange(userWorkspaces);
            dbContext.SaveChanges();
        }
    }
}