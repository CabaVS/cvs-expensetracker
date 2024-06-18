using CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainTransferTransaction = CabaVS.ExpenseTracker.Domain.Entities.TransferTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class TransferTransaction
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }

    public Guid SourceId { get; set; }
    public Balance Source { get; set; } = default!;
    public decimal AmountInSourceCurrency { get; set; }
    
    public Guid DestinationId { get; set; }
    public Balance Destination { get; set; } = default!;
    public decimal AmountInDestinationCurrency { get; set; }
    
    public string[] Tags { get; set; } = default!;
    
    public static TransferTransaction FromDomain(DomainTransferTransaction domain, Guid workspaceId)
    {
        return new TransferTransaction
        {
            Id = domain.Id,
            Date = domain.Date,
            
            AmountInSourceCurrency = domain.AmountInSourceCurrency,
            SourceId = domain.Source.Id,
            Source = Balance.FromDomain(domain.Source, workspaceId),
            
            AmountInDestinationCurrency = domain.AmountInDestinationCurrency,
            DestinationId = domain.Destination.Id,
            Destination = Balance.FromDomain(domain.Destination, workspaceId),
            
            Tags = domain.Tags.Select(x => x.Value).ToArray()
        };
    }
}

internal sealed class TransferTransactionTypeConfiguration : IEntityTypeConfiguration<TransferTransaction>
{
    public void Configure(EntityTypeBuilder<TransferTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired()
            .HasConversion<DateOnlyConverter>();

        builder.HasOne(x => x.Source)
            .WithMany()
            .HasForeignKey(x => x.SourceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // SQL Server limitations
        
        builder.Property(x => x.AmountInSourceCurrency)
            .IsRequired();
        
        builder.HasOne(x => x.Destination)
            .WithMany()
            .HasForeignKey(x => x.DestinationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // SQL Server limitations
        
        builder.Property(x => x.AmountInDestinationCurrency)
            .IsRequired();
        
        builder.Property(x => x.Tags)
            .IsRequired()
            .HasConversion<ArrayToCommaSeparatedStringsConverter>();
    }
}