using CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class User : IAuditableEntity
{
    public Guid Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    
    public Domain.Entities.User ConvertToDomain()
    {
        return new Domain.Entities.User(Id);
    }

    public static User ConvertFromDomain(Domain.Entities.User user)
    {
        return new User
        {
            Id = user.Id
        };
    }
}

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
    }
}