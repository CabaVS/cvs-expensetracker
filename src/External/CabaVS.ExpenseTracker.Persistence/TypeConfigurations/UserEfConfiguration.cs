using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.TypeConfigurations;

internal sealed class UserEfConfiguration : IEntityTypeConfiguration<UserEf>
{
    public void Configure(EntityTypeBuilder<UserEf> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn)
            .IsRequired();
        builder.Property(x => x.ModifiedOn)
            .IsRequired();
        
        builder.Property(x => x.UserName)
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();
        builder.Property(x => x.IsAdmin)
            .IsRequired();
    }
}
