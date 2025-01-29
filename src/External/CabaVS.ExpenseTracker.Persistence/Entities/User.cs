using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class User : IRepresentAuditableEntity<Domain.Entities.User, User>
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    
    public Domain.Entities.User ToDomainEntity() => new(Id, CreatedOn, ModifiedOn);

    public User FromDomainEntity(Domain.Entities.User domainEntity)
    {
        Id = domainEntity.Id;
        CreatedOn = domainEntity.CreatedOn;
        ModifiedOn = domainEntity.ModifiedOn;
        
        return this;
    }
}

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .HasMany<UserWorkspace>()
            .WithOne(uw => uw.User)
            .HasForeignKey(uw => uw.UserId);
    }
}
