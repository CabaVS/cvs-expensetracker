using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Category : IRepresentAuditableEntity<Domain.Entities.Category, Category>
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = null!;
    public CategoryType Type { get; set; }
    
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    
    public Domain.Entities.Category ToDomainEntity() => 
        Domain.Entities.Category
            .Create(
                Id,
                CreatedOn, 
                ModifiedOn,
                Name,
                Type,
                Currency.ToDomainEntity(),
                Workspace.ToDomainEntity())
            .Value;

    public Category FromDomainEntity(Domain.Entities.Category domainEntity)
    {
        Id = domainEntity.Id;
        CreatedOn = domainEntity.CreatedOn;
        ModifiedOn = domainEntity.ModifiedOn;
        Name = domainEntity.Name.Value;
        Type = domainEntity.Type;
        CurrencyId = domainEntity.Currency.Id;
        WorkspaceId = domainEntity.Workspace.Id;
        
        return this;
    }
}

internal sealed class ExpenseCategoryTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
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
