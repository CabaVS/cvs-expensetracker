using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainIncomeCategory = CabaVS.ExpenseTracker.Domain.Entities.IncomeCategory;
using DomainCurrency = CabaVS.ExpenseTracker.Domain.Entities.Currency;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class IncomeCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = default!;

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = default!;
    
    public DomainIncomeCategory ToDomain()
    {
        var currency = DomainCurrency
            .Create(Currency.Id, Currency.Name, Currency.Code, Currency.Symbol)
            .Value;
        
        return DomainIncomeCategory
            .Create(Id, Name, currency)
            .Value;
    }

    public static IncomeCategory FromDomain(DomainIncomeCategory incomeCategory, Guid workspaceId)
    {
        return new IncomeCategory
        {
            Id = incomeCategory.Id,
            Name = incomeCategory.Name.Value,
            CurrencyId = incomeCategory.Currency.Id,
            Currency = Currency.FromDomain(incomeCategory.Currency),
            WorkspaceId = workspaceId
        };
    }
}

internal sealed class IncomeCategoryTypeConfiguration : IEntityTypeConfiguration<IncomeCategory>
{
    public void Configure(EntityTypeBuilder<IncomeCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CategoryName.MaxLength);

        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}