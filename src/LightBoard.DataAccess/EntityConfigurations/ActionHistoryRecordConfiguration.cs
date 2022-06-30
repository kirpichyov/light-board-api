using LightBoard.Domain.Entities.Record;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class ActionHistoryRecordConfiguration : EntityConfigurationBase<ActionHistoryRecord, Guid>
{
    public override void Configure(EntityTypeBuilder<ActionHistoryRecord> builder)
    {
        base.Configure(builder);

        builder.Property(actionHistoryRecord => actionHistoryRecord.ResourceType).IsRequired();
        builder.Property(actionHistoryRecord => actionHistoryRecord.ActionType).IsRequired();
        builder.Property(actionHistoryRecord => actionHistoryRecord.CreatedTime).IsRequired();
        builder.Property(actionHistoryRecord => actionHistoryRecord.ResourceId).IsRequired();
        builder.Property(actionHistoryRecord => actionHistoryRecord.OldValue);
        builder.Property(actionHistoryRecord => actionHistoryRecord.NewValue);
        builder.Property(actionHistoryRecord => actionHistoryRecord.UserId).IsRequired();
    }
}