using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainWorkspace = CabaVS.ExpenseTracker.Domain.Entities.Workspace;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class Workspace
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    
    public ICollection<UserWorkspace> UserWorkspaces { get; set; } = default!;

    public static Workspace FromDomain(DomainWorkspace workspace, Guid userId)
    {
        return new Workspace
        {
            Id = workspace.Id,
            Name = workspace.Name.Value,
            UserWorkspaces = [ new UserWorkspace { WorkspaceId = workspace.Id, UserId = userId, IsAdmin = workspace.IsAdmin } ]
        };
    }
}

internal sealed class WorkspaceTypeConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(WorkspaceName.MaxLength)
            .IsRequired();

        builder.HasMany(x => x.UserWorkspaces)
            .WithOne(x => x.Workspace)
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}