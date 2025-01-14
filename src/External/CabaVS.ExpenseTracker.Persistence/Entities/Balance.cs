using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Balance : IAuditableEntity
{
    public Guid Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    
    public string Name { get; set; } = default!;
    public decimal Amount { get; set; }

    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = default!;

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = default!;

    public Domain.Entities.Balance ConvertToDomain() =>
        Domain.Entities.Balance
            .Create(
                Id, 
                Name, 
                Amount, 
                Domain.Entities.Currency
                    .Create(Currency.Id, Currency.Name, Currency.Code, Currency.Symbol)
                    .Value)
            .Value;

    public static Balance ConvertFromDomain(Domain.Entities.Balance balance, Guid workspaceId) =>
        new()
        {
            Id = balance.Id,
            Name = balance.Name.Value,
            Amount = balance.Amount,
            CurrencyId = balance.Currency.Id,
            WorkspaceId = workspaceId
        };
}

internal sealed class BalanceConfiguration : IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(BalanceName.MaxLength);

        builder.Property(x => x.Amount)
            .IsRequired();
        
        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
