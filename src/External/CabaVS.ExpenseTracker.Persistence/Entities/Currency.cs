using CabaVS.ExpenseTracker.Domain.ValueObjects;
using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class Currency : IAuditableEntity
{
    public Guid Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    
    public Domain.Entities.Currency ConvertToDomain() =>
        Domain.Entities.Currency
            .Create(Id, Name, Code, Symbol)
            .Value;

    public static Currency ConvertFromDomain(Domain.Entities.Currency currency) =>
        new()
        {
            Id = currency.Id,
            Name = currency.Name.Value,
            Code = currency.Code.Value,
            Symbol = currency.Symbol.Value
        };
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
