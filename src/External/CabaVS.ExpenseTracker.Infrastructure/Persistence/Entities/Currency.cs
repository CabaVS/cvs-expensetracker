using CabaVS.ExpenseTracker.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainCurrency = CabaVS.ExpenseTracker.Domain.Entities.Currency;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;

internal sealed class Currency
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Symbol { get; set; } = default!;

    public DomainCurrency ToDomain(Currency currency)
    {
        return DomainCurrency
            .Create(Id, Name, Code, Symbol)
            .Value;
    }

    public static Currency FromDomain(DomainCurrency currency)
    {
        return new Currency
        {
            Id = currency.Id,
            Name = currency.Name.Value,
            Code = currency.Code.Value,
            Symbol = currency.Symbol.Value
        };
    }
}

internal sealed class CurrencyTypeConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(CurrencyName.MaxLength)
            .IsRequired();
        
        builder.Property(x => x.Code)
            .HasMaxLength(CurrencyCode.MaxLength)
            .IsRequired();
        
        builder.Property(x => x.Symbol)
            .HasMaxLength(CurrencySymbol.MaxLength)
            .IsRequired();
    }
}