using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using CabaVS.ExpenseTracker.IntegrationTests.Common;

namespace CabaVS.ExpenseTracker.IntegrationTests.DatabaseSeeders;

internal static class DefaultSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        SeedUsers();
        SeedWorkspaces();
        BindUsersToWorkspaces();
        return;

        void SeedUsers()
        {
            var authenticatedUser = new User { Id = SharedState.Instance[StateKeys.AuthenticatedUser] };
            dbContext.Users.Add(authenticatedUser);

            var users = Enumerable.Range(1, 4)
                .Select(x =>
                {
                    var user = new User { Id = Guid.NewGuid() };
                    SharedState.Instance.Add($"User_{x}", user.Id);
                    return user;
                });
            
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }

        void SeedWorkspaces()
        {
            var workspaces = Enumerable.Range(1, 8)
                .Select(x =>
                {
                    var workspace = new Workspace
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Workspace {x}"
                    };
                    
                    SharedState.Instance.Add($"Workspace_{x}", workspace.Id);
                    return workspace;
                });
            
            dbContext.Workspaces.AddRange(workspaces);
            dbContext.SaveChanges();
        }

        void BindUsersToWorkspaces()
        {
            var userWorkspaces = Enumerable.Range(1, 4)
                .SelectMany(x => new UserWorkspace[]
                {
                    new()
                    {
                        UserId = SharedState.Instance[$"User_{x}"],
                        WorkspaceId = SharedState.Instance[$"Workspace_{x * 2 - 1}"],
                        IsAdmin = false
                    },
                    new()
                    {
                        UserId = SharedState.Instance[$"User_{x}"],
                        WorkspaceId = SharedState.Instance[$"Workspace_{x * 2}"],
                        IsAdmin = true
                    }
                });
            
            dbContext.UserWorkspaces.AddRange(userWorkspaces);
            dbContext.SaveChanges();
        }
    }

    
}