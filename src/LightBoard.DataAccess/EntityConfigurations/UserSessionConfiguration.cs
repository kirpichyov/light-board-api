using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class UserSessionConfiguration : EntityConfigurationBase<UserSession, string>
{
    public override void Configure(EntityTypeBuilder<UserSession> builder)
    {
        base.Configure(builder);

        builder.Property(session => session.UserId).IsRequired();
        builder.Property(session => session.CreatedAtUtc).IsRequired();
        builder.Property(session => session.ExpiresAtUtc).IsRequired();

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(session => session.UserId);
    }
}