using LightBoard.Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class CardAssigneeConfiguration : EntityConfigurationBase<CardAssignee, Guid>
{
    public override void Configure(EntityTypeBuilder<CardAssignee> builder)
    {
        base.Configure(builder);
        
        builder.Property(cardAssignee => cardAssignee.CardId).IsRequired();
        builder.Property(cardAssignee => cardAssignee.UserId).IsRequired();
        
        builder.HasOne(cardAssignee => cardAssignee.Card)
            .WithMany(cardAssignee => cardAssignee.CardAssignees)
            .HasForeignKey(cardAssignee => cardAssignee.CardId);

        builder.HasOne(cardAssignee => cardAssignee.User)
            .WithMany()
            .HasForeignKey(cardAssignee => cardAssignee.UserId);
    }
}