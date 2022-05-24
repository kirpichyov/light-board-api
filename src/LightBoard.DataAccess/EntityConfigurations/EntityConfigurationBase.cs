using LightBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightBoard.DataAccess.EntityConfigurations;

public abstract class EntityConfigurationBase<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase<TKey>
    where TKey : struct 
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
    }
}