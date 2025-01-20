using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Workspace
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    
    public string Name { get; set; } = default!;

    public Domain.Entities.Workspace ConvertToDomainEntity() => 
        Domain.Entities.Workspace
            .Create(
                Id,
                CreatedOn,
                ModifiedOn,
                Name)
            .Value;

    public static Workspace ConvertFromDomainEntity(Domain.Entities.Workspace domainEntity) =>
        new()
        {
            Id = domainEntity.Id,
            CreatedOn = domainEntity.CreatedOn,
            ModifiedOn = domainEntity.ModifiedOn,
            Name = domainEntity.Name.Value
        };
}

internal sealed class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(WorkspaceName.MaxLength);
    }
}
