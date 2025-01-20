using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Models;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class ExpenseCategory
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public string Name { get; set; } = null!;
    
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public Domain.Entities.ExpenseCategory ConvertToDomainEntity() =>
        Domain.Entities.ExpenseCategory
            .Create(
                Id, 
                CreatedOn,
                ModifiedOn,
                Name, 
                Currency.ConvertToDomainEntity())
            .Value;

    public static ExpenseCategory ConvertFromDomainEntity(Domain.Entities.ExpenseCategory domainEntity, Guid workspaceId) =>
        new()
        {
            Id = domainEntity.Id,
            CreatedOn = domainEntity.CreatedOn,
            ModifiedOn = domainEntity.ModifiedOn,
            Name = domainEntity.Name.Value,
            CurrencyId = domainEntity.Currency.Id,
            WorkspaceId = workspaceId
        };
    
    public static Expression<Func<ExpenseCategory, ExpenseCategoryModel>> ProjectToModel() =>
        ec => new ExpenseCategoryModel(
            ec.Id, ec.Name, Currency.ConvertToModel(ec.Currency));
}

internal sealed class ExpenseCategoryTypeConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.HasKey(ec => ec.Id);

        builder.Property(ec => ec.Name)
            .HasMaxLength(CategoryName.MaxLength)
            .IsRequired();

        builder.HasOne(ec => ec.Currency)
            .WithMany()
            .HasForeignKey(ec => ec.CurrencyId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ec => ec.Workspace)
            .WithMany()
            .HasForeignKey(ec => ec.WorkspaceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
