using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.TypeConfigurations;

internal sealed class CurrencyEfConfiguration : IEntityTypeConfiguration<CurrencyEf>
{
    public void Configure(EntityTypeBuilder<CurrencyEf> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CreatedOn)
            .IsRequired();
        builder.Property(x => x.ModifiedOn)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(CurrencyName.MaxLength)
            .IsRequired();
        builder.Property(x => x.Code)
            .HasMaxLength(CurrencyCode.MaxLength)
            .IsRequired();
        builder.Property(x => x.Symbol)
            .HasMaxLength(CurrencySymbol.MaxLength)
            .IsRequired();

        builder.ToTable(
            build => build.HasCheckConstraint(
                "CK_CurrencySymbol_MinLength",
                $"LEN(Code) >= {CurrencyCode.MinLength}"));
    }
}
