using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Currency : IRepresentAuditableEntity<Domain.Entities.Currency, Currency>
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Symbol { get; set; } = default!;

    public Domain.Entities.Currency ToDomainEntity() =>
        Domain.Entities.Currency
            .Create(Id, CreatedOn, ModifiedOn, Name, Code, Symbol)
            .Value;

    public Currency FromDomainEntity(Domain.Entities.Currency domainEntity)
    {
        Id = domainEntity.Id;
        CreatedOn = domainEntity.CreatedOn;
        ModifiedOn = domainEntity.ModifiedOn;
        Name = domainEntity.Name.Value;
        Code = domainEntity.Code.Value;
        Symbol = domainEntity.Symbol.Value;
        
        return this;
    }
}

internal sealed class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CurrencyName.MaxLength);
        
        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(CurrencyCode.MaxLength);
        
        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(CurrencySymbol.MaxLength);
    }
}
