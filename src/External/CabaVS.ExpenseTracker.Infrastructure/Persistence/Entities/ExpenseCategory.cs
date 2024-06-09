using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainExpenseCategory = CabaVS.ExpenseTracker.Domain.Entities.ExpenseCategory;
using DomainCurrency = CabaVS.ExpenseTracker.Domain.Entities.Currency;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class ExpenseCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = default!;

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = default!;
    
    public DomainExpenseCategory ToDomain(ExpenseCategory expenseCategory)
    {
        var currency = DomainCurrency
            .Create(expenseCategory.Currency.Id, expenseCategory.Currency.Name, expenseCategory.Currency.Code, expenseCategory.Currency.Symbol)
            .Value;
        
        return DomainExpenseCategory
            .Create(Id, Name, currency)
            .Value;
    }

    public static ExpenseCategory FromDomain(DomainExpenseCategory expenseCategory, Guid workspaceId)
    {
        return new ExpenseCategory
        {
            Id = expenseCategory.Id,
            Name = expenseCategory.Name.Value,
            CurrencyId = expenseCategory.Currency.Id,
            WorkspaceId = workspaceId
        };
    }
}

internal sealed class ExpenseCategoryTypeConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
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