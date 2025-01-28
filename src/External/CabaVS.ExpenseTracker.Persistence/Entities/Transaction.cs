using CabaVS.ExpenseTracker.Domain.Entities.Abstractions;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Persistence.Converters;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Transaction : IRepresentAuditableEntity<Domain.Entities.Transaction, Transaction>
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public DateOnly Date { get; set; }
    public string[] Tags { get; set; } = null!;
    public TransactionType Type { get; set; }
    
    public decimal AmountInSourceCurrency { get; set; }
    public decimal AmountInDestinationCurrency { get; set; }
    
    public Workspace Workspace { get; set; } = null!;
    public Guid WorkspaceId { get; set; }
    
    public Balance? SourceBalance { get; set; }
    public Guid? SourceBalanceId { get; set; }
    
    public Balance? DestinationBalance { get; set; }
    public Guid? DestinationBalanceId { get; set; }
    
    public Category? SourceCategory { get; set; }
    public Guid? SourceCategoryId { get; set; }
    
    public Category? DestinationCategory { get; set; }
    public Guid? DestinationCategoryId { get; set; }

    public Domain.Entities.Transaction ToDomainEntity()
    {
        (IWithCurrency source, IWithCurrency destination) = DetermineSourceAndDestination();
        
        return Domain.Entities.Transaction
            .Create(
                Id,
                CreatedOn,
                ModifiedOn,
                Date,
                Tags,
                AmountInSourceCurrency,
                AmountInDestinationCurrency,
                Type,
                source,
                destination,
                Workspace.ToDomainEntity())
            .Value;
    }

    public Transaction FromDomainEntity(Domain.Entities.Transaction domainEntity)
    {
        Id = domainEntity.Id;
        CreatedOn = domainEntity.CreatedOn;
        ModifiedOn = domainEntity.ModifiedOn;
        Date = domainEntity.Date;
        Tags = domainEntity.Tags;
        AmountInSourceCurrency = domainEntity.AmountInSourceCurrency;
        AmountInDestinationCurrency = domainEntity.AmountInDestinationCurrency;
        Type = domainEntity.Type;
        WorkspaceId = domainEntity.Workspace.Id;

        switch (Type)
        {
            case TransactionType.Income:
                SourceCategoryId = ((Domain.Entities.Category)domainEntity.Source).Id;
                DestinationBalanceId = ((Domain.Entities.Balance)domainEntity.Destination).Id;
                break;
            case TransactionType.Expense:
                SourceBalanceId = ((Domain.Entities.Balance)domainEntity.Source).Id;
                DestinationCategoryId = ((Domain.Entities.Category)domainEntity.Destination).Id;
                break;
            case TransactionType.Transfer:
                SourceBalanceId = ((Domain.Entities.Balance)domainEntity.Source).Id;
                DestinationBalanceId = ((Domain.Entities.Balance)domainEntity.Destination).Id;
                break;
            default:
                throw new InvalidOperationException();
        }
        
        return this;
    }

    private (IWithCurrency Source, IWithCurrency Destination) DetermineSourceAndDestination() =>
        Type switch
        {
            TransactionType.Income => (SourceCategory!.ToDomainEntity(), DestinationBalance!.ToDomainEntity()),
            TransactionType.Expense => (SourceBalance!.ToDomainEntity(), DestinationCategory!.ToDomainEntity()),
            TransactionType.Transfer => (SourceBalance!.ToDomainEntity(), DestinationBalance!.ToDomainEntity()),
            _ => throw new InvalidOperationException()
        };
}

internal sealed class TransferTransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn).IsRequired();
        builder.Property(x => x.ModifiedOn).IsRequired();
        
        builder
            .Property(x => x.Date)
            .HasConversion(new DateOnlyToDateTimeConverter())
            .IsRequired();
        builder
            .Property(x => x.Type)
            .IsRequired();
        
        builder
            .Property(x => x.Tags)
            .HasConversion(new TagsCollectionToCommaSeparatedStringConverter())
            .IsRequired();
        builder.Property(x => x.Tags)
            .Metadata
            .SetValueComparer(new TagsCollectionToCommaSeparatedStringComparer());

        builder.Property(x => x.AmountInSourceCurrency).IsRequired();
        builder.Property(x => x.AmountInDestinationCurrency).IsRequired();
        
        builder
            .HasOne(x => x.SourceBalance)
            .WithMany()
            .HasForeignKey(x => x.SourceBalanceId)
            .OnDelete(DeleteBehavior.NoAction) // Limitation 'multiple cascade paths' of SQL Server
            .IsRequired(false);
        builder
            .HasOne(x => x.DestinationBalance)
            .WithMany()
            .HasForeignKey(x => x.DestinationBalanceId)
            .OnDelete(DeleteBehavior.NoAction) // Limitation 'multiple cascade paths' of SQL Server
            .IsRequired(false);
        builder
            .HasOne(x => x.SourceCategory)
            .WithMany()
            .HasForeignKey(x => x.SourceCategoryId)
            .OnDelete(DeleteBehavior.NoAction) // Limitation 'multiple cascade paths' of SQL Server
            .IsRequired(false);
        builder
            .HasOne(x => x.DestinationCategory)
            .WithMany()
            .HasForeignKey(x => x.DestinationCategoryId)
            .OnDelete(DeleteBehavior.NoAction) // Limitation 'multiple cascade paths' of SQL Server
            .IsRequired(false);
    }
}
