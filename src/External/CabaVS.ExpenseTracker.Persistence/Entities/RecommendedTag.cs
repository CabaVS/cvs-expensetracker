using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class RecommendedTag
{
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public TagType Type { get; set; }
    
    internal enum TagType
    {
        TransferTransaction,
        IncomeTransaction,
        ExpenseTransaction,
    }
}

internal sealed class RecommendedTagEntityConfiguration : IEntityTypeConfiguration<RecommendedTag>
{
    public void Configure(EntityTypeBuilder<RecommendedTag> builder)
    {
        builder.HasKey(x => new { x.WorkspaceId, x.Type, x.Name });
        
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        
        builder
            .HasOne(x => x.Workspace)
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}