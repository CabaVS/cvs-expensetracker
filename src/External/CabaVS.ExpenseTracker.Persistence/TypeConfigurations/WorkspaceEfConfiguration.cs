using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.TypeConfigurations;

internal sealed class WorkspaceEfConfiguration : IEntityTypeConfiguration<WorkspaceEf>
{
    public void Configure(EntityTypeBuilder<WorkspaceEf> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn)
            .IsRequired();
        builder.Property(x => x.ModifiedOn)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(WorkspaceName.MaxLength)
            .IsRequired();
    }
}
