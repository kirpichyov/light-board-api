using LightBoard.Domain.Entities.Cards;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations
{
    public class CardCommentCofiguration : EntityConfigurationBase<CardComment, Guid>
    {
        public override void Configure(EntityTypeBuilder<CardComment> builder)
        {
            base.Configure(builder);

            builder.Property(comment => comment.UserId).IsRequired();
            builder.Property(comment => comment.CardId).IsRequired();
            builder.Property(comment => comment.Message).IsRequired();
            builder.Property(comment => comment.CreatedAtUtc).IsRequired();

            builder.HasOne(comment => comment.User)
                .WithMany()
                .HasForeignKey(comment => comment.UserId);

            builder.HasOne(comment => comment.Card)
                .WithMany(comment => comment.Comments)
                .HasForeignKey(comment => comment.CardId);
        }
    }
}
