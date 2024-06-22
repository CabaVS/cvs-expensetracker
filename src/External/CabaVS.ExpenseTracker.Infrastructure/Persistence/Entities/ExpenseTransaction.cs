using CabaVS.ExpenseTracker.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainExpenseTransaction = CabaVS.ExpenseTracker.Domain.Entities.ExpenseTransaction;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class ExpenseTransaction
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }

    public Guid SourceId { get; set; }
    public Balance Source { get; set; } = default!;
    public decimal AmountInSourceCurrency { get; set; }
    
    public Guid DestinationId { get; set; }
    public ExpenseCategory Destination { get; set; } = default!;
    public decimal AmountInDestinationCurrency { get; set; }

    public string[] Tags { get; set; } = default!;

    public DomainExpenseTransaction ToDomain()
    {
        return DomainExpenseTransaction
            .Create(
                Id,
                Date,
                Domain.Entities.Balance
                    .Create(
                        Source.Id,
                        Source.Name,
                        Source.Amount,
                        Domain.Entities.Currency
                            .Create(
                                Source.Currency.Id,
                                Source.Currency.Name,
                                Source.Currency.Code,
                                Source.Currency.Symbol)
                            .Value)
                    .Value,
                Domain.Entities.ExpenseCategory
                    .Create(
                        Destination.Id,
                        Destination.Name,
                        Domain.Entities.Currency
                            .Create(
                                Destination.Currency.Id,
                                Destination.Currency.Name,
                                Destination.Currency.Code,
                                Destination.Currency.Symbol)
                            .Value)
                    .Value,
                AmountInSourceCurrency,
                AmountInDestinationCurrency,
                Tags,
                false)
            .Value;
    }
    
    public static ExpenseTransaction FromDomain(DomainExpenseTransaction domain, Guid workspaceId)
    {
        return new ExpenseTransaction
        {
            Id = domain.Id,
            Date = domain.Date,
            
            AmountInSourceCurrency = domain.AmountInSourceCurrency,
            SourceId = domain.Source.Id,
            Source = Balance.FromDomain(domain.Source, workspaceId),
            
            AmountInDestinationCurrency = domain.AmountInDestinationCurrency,
            DestinationId = domain.Destination.Id,
            Destination = ExpenseCategory.FromDomain(domain.Destination, workspaceId),
            
            Tags = domain.Tags.Select(x => x.Value).ToArray()
        };
    }
}

internal sealed class ExpenseTransactionTypeConfiguration : IEntityTypeConfiguration<ExpenseTransaction>
{
    public void Configure(EntityTypeBuilder<ExpenseTransaction> builder)
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