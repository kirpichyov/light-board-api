using LightBoard.Domain.Entities.Boards;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public class BoardMemberConfiguration : EntityConfigurationBase<BoardMember, Guid>
{
    public override void Configure(EntityTypeBuilder<BoardMember> builder)
    {
        base.Configure(builder);

        builder.Property(boardMember => boardMember.BoardId).IsRequired();
        builder.Property(boardMember => boardMember.UserId).IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Board)
            .WithMany(x => x.BoardMembers)  
            .HasForeignKey(x => x.BoardId);
    }
}