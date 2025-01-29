using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Workspace : IRepresentAuditableEntity<Domain.Entities.Workspace, Workspace>
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    
    public string Name { get; set; } = default!;

    public Domain.Entities.Workspace ToDomainEntity() =>
        Domain.Entities.Workspace
            .Create(Id, CreatedOn, ModifiedOn, Name)
            .Value;

    public Workspace FromDomainEntity(Domain.Entities.Workspace domainEntity)
    {
        Id = domainEntity.Id;
        CreatedOn = domainEntity.CreatedOn;
        ModifiedOn = domainEntity.ModifiedOn;
        Name = domainEntity.Name.Value;
        
        return this;
    }
}

internal sealed class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(WorkspaceName.MaxLength);
        
        builder
            .HasMany<UserWorkspace>()
            .WithOne(uw => uw.Workspace)
            .HasForeignKey(uw => uw.WorkspaceId);
    }
}
