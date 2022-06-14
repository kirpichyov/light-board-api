using LightBoard.Domain.Entities.Columns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class ColumnConfiguration : EntityConfigurationBase<Column, Guid>
{
    public override void Configure(EntityTypeBuilder<Column> builder)
    {
        base.Configure(builder);
        
        builder.Property(column => column.BoardId).IsRequired();
        builder.Property(column => column.Name).IsRequired();
        builder.Property(column => column.Order).IsRequired();

        builder.HasOne(x => x.Board)
            .WithMany(board => board.Columns)
            .HasForeignKey(x => x.BoardId);
    }
}