using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class UserWorkspace
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = default!;

    public bool IsAdmin { get; set; }
}

internal sealed class UserWorkspaceTypeConfiguration : IEntityTypeConfiguration<UserWorkspace>
{
    public void Configure(EntityTypeBuilder<UserWorkspace> builder)
    {
        builder.HasKey(x => new { x.UserId, x.WorkspaceId });
    }
}