using CabaVS.ExpenseTracker.Persistence.Converters;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.TypeConfigurations;

internal sealed class TransactionEfConfiguration : IEntityTypeConfiguration<TransactionEf>
{
    public void Configure(EntityTypeBuilder<TransactionEf> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn)
            .IsRequired();
        builder.Property(x => x.ModifiedOn)
            .IsRequired();

        builder.Property(x => x.Date)
            .HasConversion(new DateOnlyToDateTimeConverter())
            .IsRequired();
        builder.Property(x => x.Type)
            .IsRequired();
        builder.Property(x => x.Tags)
            .HasConversion(new StringArrayToCommaSeparatedStringConverter())
            .IsRequired()
            .Metadata
            .SetValueComparer(new StringArrayToCommaSeparatedStringComparer());

        builder.Property(x => x.AmountInSourceCurrency)
            .IsRequired();
        builder.Property(x => x.AmountInDestinationCurrency)
            .IsRequired();
        
        builder.HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(x => x.SourceBalance)
            .WithMany()
            .HasForeignKey(x => x.SourceBalanceId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        
        builder.HasOne(x => x.DestinationBalance)
            .WithMany()
            .HasForeignKey(x => x.DestinationBalanceId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        
        builder.HasOne(x => x.SourceCategory)
            .WithMany()
            .HasForeignKey(x => x.SourceCategoryId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        
        builder.HasOne(x => x.DestinationCategory)
            .WithMany()
            .HasForeignKey(x => x.DestinationCategoryId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
    }
}
