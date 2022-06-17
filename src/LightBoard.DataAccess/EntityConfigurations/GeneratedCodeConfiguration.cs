using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class GeneratedCodeConfiguration : EntityConfigurationBase<CodeBase, Guid>
{
    public override void Configure(EntityTypeBuilder<CodeBase> builder)
    {
        base.Configure(builder);

        builder.Property(passwordCode => passwordCode.Email).IsRequired();
        builder.Property(passwordCode => passwordCode.ResetCode).IsRequired();
        builder.Property(passwordCode => passwordCode.ExpirationDate).IsRequired();
    }
}