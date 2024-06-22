using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<UserWorkspace> UserWorkspaces { get; set; }
    
    public DbSet<Currency> Currencies { get; set; }

    public DbSet<Balance> Balances { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<IncomeCategory> IncomeCategories { get; set; }
    public DbSet<ExpenseTransaction> ExpenseTransactions { get; set; }
    public DbSet<IncomeTransaction> IncomeTransactions { get; set; }
    public DbSet<TransferTransaction> TransferTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new TransactionsRemovalInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyMarker.Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 2);
    }
}