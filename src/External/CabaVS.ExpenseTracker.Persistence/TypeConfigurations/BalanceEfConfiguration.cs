using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.TypeConfigurations;

internal sealed class BalanceEfConfiguration : IEntityTypeConfiguration<BalanceEf>
{
    public void Configure(EntityTypeBuilder<BalanceEf> builder)
    {
        builder.ToTable("Balances");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn)
            .IsRequired();
        builder.Property(x => x.ModifiedOn)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(BalanceName.MaxLength)
            .IsRequired();
        
        builder.HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
