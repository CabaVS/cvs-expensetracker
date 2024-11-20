using CabaVS.ExpenseTracker.IntegrationTests.Fakers;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Persistence.Entities;

namespace CabaVS.ExpenseTracker.IntegrationTests.Seeders;

internal static class DefaultDatabaseSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        using var transaction = dbContext.Database.BeginTransaction();
        
        SeedUsers(numberOfUsers: 4);
        SeedWorkspaces(numberOfWorkspacesPerUser: 2, excludeUsers: [CurrentUserAccessorInjected.AuthenticatedUserId]);
        
        transaction.Commit();
        return;

        void SeedUsers(int numberOfUsers)
        {
            var generatedUsers = new UserFaker().Generate(numberOfUsers);
            generatedUsers.Add(new UserFaker(CurrentUserAccessorInjected.AuthenticatedUserId).Generate());
            
            dbContext.Users.AddRange(generatedUsers);
            dbContext.SaveChanges();
        }

        void SeedWorkspaces(int numberOfWorkspacesPerUser, Guid[]? excludeUsers = null)
        {
            var users = dbContext.Users
                .Where(u => excludeUsers != null && !excludeUsers.Contains(u.Id))
                .ToArray();

            var workspaceFaker = new WorkspaceFaker();
            foreach (var user in users)
            {
                var generatedWorkspaces = workspaceFaker.Generate(numberOfWorkspacesPerUser);
                dbContext.Workspaces.AddRange(generatedWorkspaces);

                foreach (var workspaces in generatedWorkspaces.Chunk(numberOfWorkspacesPerUser))
                {
                    var midPoint = workspaces.Length / 2;
                    var adminBinding = workspaces
                        .Take(midPoint)
                        .Select(w => new UserWorkspace { WorkspaceId = w.Id, UserId = user.Id, IsAdmin = true });
                    var nonAdminBinding = workspaces
                        .Skip(midPoint)
                        .Take(workspaces.Length - midPoint)
                        .Select(w => new UserWorkspace { WorkspaceId = w.Id, UserId = user.Id, IsAdmin = false });
                    
                    dbContext.UserWorkspaces.AddRange(adminBinding.Concat(nonAdminBinding));
                }
            }
            
            dbContext.SaveChanges();
        }
    }
}