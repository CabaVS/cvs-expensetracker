using CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainIncomeTransaction = CabaVS.ExpenseTracker.Domain.Entities.IncomeTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class IncomeTransaction
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }

    public Guid SourceId { get; set; }
    public IncomeCategory Source { get; set; } = default!;
    public decimal AmountInSourceCurrency { get; set; }
    
    public Guid DestinationId { get; set; }
    public Balance Destination { get; set; } = default!;
    public decimal AmountInDestinationCurrency { get; set; }

    public static IncomeTransaction FromDomain(DomainIncomeTransaction domain, Guid workspaceId)
    {
        return new IncomeTransaction
        {
            Id = domain.Id,
            Date = domain.Date,
            
            AmountInSourceCurrency = domain.AmountInSourceCurrency,
            SourceId = domain.Source.Id,
            Source = IncomeCategory.FromDomain(domain.Source, workspaceId),
            
            AmountInDestinationCurrency = domain.AmountInDestinationCurrency,
            DestinationId = domain.Destination.Id,
            Destination = Balance.FromDomain(domain.Destination, workspaceId)
        };
    }
}

internal sealed class IncomeTransactionTypeConfiguration : IEntityTypeConfiguration<IncomeTransaction>
{
    public void Configure(EntityTypeBuilder<IncomeTransaction> builder)
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
    }
}