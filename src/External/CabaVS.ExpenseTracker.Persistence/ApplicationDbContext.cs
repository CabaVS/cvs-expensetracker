using CabaVS.ExpenseTracker.Persistence.Entities;
using CabaVS.ExpenseTracker.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserWorkspace> UserWorkspaces { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    
    public DbSet<Currency> Currencies { get; set; }

    public DbSet<Balance> Balances { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.AddInterceptors(
            new AuditableEntityInterceptor(),
            new TransactionsRemovalInterceptor());

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
}
