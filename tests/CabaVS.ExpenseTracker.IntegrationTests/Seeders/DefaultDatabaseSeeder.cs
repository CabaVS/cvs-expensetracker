using CabaVS.ExpenseTracker.IntegrationTests.Fakers;
using CabaVS.ExpenseTracker.IntegrationTests.Injected;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace CabaVS.ExpenseTracker.IntegrationTests.Seeders;

internal static class DefaultDatabaseSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
        
        SeedCurrencies(numberOfCurrencies: 4);
        
        SeedUsers(numberOfUsers: 4);
        SeedWorkspaces(numberOfWorkspacesPerUser: 2, excludeUsers: [CurrentUserAccessorInjected.AuthenticatedUserId]);
        
        transaction.Commit();
        return;

        void SeedCurrencies(int numberOfCurrencies)
        {
            List<Currency> generatedCurrencies = new CurrencyFaker().Generate(numberOfCurrencies);
            
            dbContext.Currencies.AddRange(generatedCurrencies);
            dbContext.SaveChanges();
        }
        
        void SeedUsers(int numberOfUsers)
        {
            List<User> generatedUsers = new UserFaker().Generate(numberOfUsers);
            generatedUsers.Add(new UserFaker(CurrentUserAccessorInjected.AuthenticatedUserId).Generate());
            
            dbContext.Users.AddRange(generatedUsers);
            dbContext.SaveChanges();
        }

        void SeedWorkspaces(int numberOfWorkspacesPerUser, Guid[]? excludeUsers = null)
        {
            User[] users = dbContext.Users
                .Where(u => excludeUsers != null && !excludeUsers.Contains(u.Id))
                .ToArray();

            var workspaceFaker = new WorkspaceFaker();
            foreach (User user in users)
            {
                List<Workspace> generatedWorkspaces = workspaceFaker.Generate(numberOfWorkspacesPerUser);
                dbContext.Workspaces.AddRange(generatedWorkspaces);

                foreach (Workspace[] workspaces in generatedWorkspaces.Chunk(numberOfWorkspacesPerUser))
                {
                    var midPoint = workspaces.Length / 2;
                    IEnumerable<UserWorkspace> adminBinding = workspaces
                        .Take(midPoint)
                        .Select(w => new UserWorkspace { WorkspaceId = w.Id, UserId = user.Id, IsAdmin = true });
                    IEnumerable<UserWorkspace> nonAdminBinding = workspaces
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
