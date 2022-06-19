using LightBoard.Domain.Entities.Attachments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations
{
    public class CardAttachmentConfiguration : EntityConfigurationBase<CardAttachment, Guid>
    {
        public override void Configure(EntityTypeBuilder<CardAttachment> builder)
        {
            base.Configure(builder);

            builder.Property(cardAttachment => cardAttachment.Name).IsRequired();
            builder.Property(cardAttachment => cardAttachment.Url).IsRequired();
            builder.Property(cardAttachment => cardAttachment.UploadedAtUtc).IsRequired();
        }
    }
}
