using LightBoard.Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class CardConfiguration : EntityConfigurationBase<Card, Guid>
{
    public override void Configure(EntityTypeBuilder<Card> builder)
    {
        base.Configure(builder);
        
        builder.Property(card => card.ColumnId).IsRequired();
        builder.Property(card => card.Title).IsRequired();
        builder.Property(card => card.Description).IsRequired();
        builder.Property(card => card.Order).IsRequired();

        builder.HasOne(card => card.Column)
            .WithMany(column => column.Cards)
            .HasForeignKey(card => card.ColumnId);
    }
}