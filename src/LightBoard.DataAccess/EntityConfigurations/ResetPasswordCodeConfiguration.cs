using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class ResetPasswordCodeConfiguration : EntityConfigurationBase<ResetPasswordCode, Guid>
{
    public override void Configure(EntityTypeBuilder<ResetPasswordCode> builder)
    {
        base.Configure(builder);

        builder.Property(passwordCode => passwordCode.Email).IsRequired();
        builder.Property(passwordCode => passwordCode.ResetCode).IsRequired();
        builder.Property(passwordCode => passwordCode.ExpirationDate).IsRequired();
    }
}