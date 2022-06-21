using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class CodeBaseConfiguration : EntityConfigurationBase<CodeBase, Guid>
{
    public override void Configure(EntityTypeBuilder<CodeBase> builder)
    {
        base.Configure(builder);
        
                
        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<ConfirmEmailCode>(nameof(ConfirmEmailCode))
            .HasValue<ResetPasswordCode>(nameof(ResetPasswordCode));
    }
}