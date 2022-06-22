using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class UserConfiguration : EntityConfigurationBase<User, Guid>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.Name).IsRequired();
        builder.Property(user => user.Surname).IsRequired();
        builder.Property(user => user.PasswordHash).IsRequired();
        builder.Property(user => user.AvatarUrl).IsRequired(false);
        builder.Property(user => user.AvatarBlobName).IsRequired(false);
    }
}