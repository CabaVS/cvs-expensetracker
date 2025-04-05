using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEf> Users { get; set; }
    
    public DbSet<WorkspaceEf> Workspaces { get; set; }
    public DbSet<WorkspaceMemberEf> WorkspaceMembers { get; set; }

    public DbSet<CurrencyEf> Currencies { get; set; }
    
    public DbSet<BalanceEf> Balances { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
}
