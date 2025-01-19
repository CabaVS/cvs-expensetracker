using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class User
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public Domain.Entities.User ConvertToDomainEntity() => 
        new(Id, CreatedOn, ModifiedOn);

    public static User ConvertFromDomainEntity(Domain.Entities.User domainEntity) =>
        new()
        {
            Id = domainEntity.Id,
            CreatedOn = domainEntity.CreatedOn,
            ModifiedOn = domainEntity.ModifiedOn
        };
}

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) => builder.HasKey(x => x.Id);
}
