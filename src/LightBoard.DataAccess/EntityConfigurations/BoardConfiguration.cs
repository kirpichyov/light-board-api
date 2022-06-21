using LightBoard.Domain.Entities.Boards;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class BoardConfiguration : EntityConfigurationBase<Board, Guid>
{
    public override void Configure(EntityTypeBuilder<Board> builder)
    {
        base.Configure(builder);

        builder.Property(board => board.Name).IsRequired();
        builder.Property(user => user.IsArchived).IsRequired();
        builder.Property(board => board.BoardBackgroundUrl).IsRequired(false);
    }
}