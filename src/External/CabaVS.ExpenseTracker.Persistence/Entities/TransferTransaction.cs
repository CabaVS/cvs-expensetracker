using CabaVS.ExpenseTracker.Persistence.Converters;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class TransferTransaction : IAuditableEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateOnly Date { get; set; }
    public string[] Tags { get; set; } = null!;

    public decimal Amount { get; set; }
    public decimal AmountInSourceCurrency { get; set; }
    public decimal AmountInDestinationCurrency { get; set; }
    
    public Currency Currency { get; set; } = null!;
    public Guid CurrencyId { get; set; }
    
    public Balance Source { get; set; } = null!;
    public Guid SourceId { get; set; }
    
    public Balance Destination { get; set; } = null!;
    public Guid DestinationId { get; set; }
}

internal sealed class TransferTransactionEntityConfiguration : IEntityTypeConfiguration<TransferTransaction>
{
    public void Configure(EntityTypeBuilder<TransferTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn).IsRequired();
        builder.Property(x => x.ModifiedOn).IsRequired(false);
        
        builder
            .Property(x => x.Date)
            .HasConversion(
                new DateOnlyToDateTimeConverter())
            .IsRequired();
        builder
            .Property(x => x.Tags)
            .HasConversion(
                new TagsCollectionToCommaSeparatedStringConverter())
            .IsRequired();
        builder.Property(x => x.Tags)
            .Metadata
            .SetValueComparer(
                new TagsCollectionToCommaSeparatedStringComparer());

        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.AmountInSourceCurrency).IsRequired();
        builder.Property(x => x.AmountInDestinationCurrency).IsRequired();

        builder
            .HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder
            .HasOne(x => x.Source)
            .WithMany()
            .HasForeignKey(x => x.SourceId)
            .OnDelete(DeleteBehavior.NoAction) // Limitation 'multiple cascade paths' of SQL Server
            .IsRequired();
        builder
            .HasOne(x => x.Destination)
            .WithMany()
            .HasForeignKey(x => x.DestinationId)
            .OnDelete(DeleteBehavior.NoAction) // Limitation 'multiple cascade paths' of SQL Server
            .IsRequired();
    }
}