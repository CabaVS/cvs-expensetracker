using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Workspace : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public Domain.Entities.Workspace ConvertToDomain()
    {
        return Domain.Entities.Workspace.Create(Id, Name).Value;
    }

    public static Workspace ConvertFromDomain(Domain.Entities.Workspace workspace)
    {
        return new Workspace
        {
            Id = workspace.Id,
            Name = workspace.Name.Value
        };
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
    }
}