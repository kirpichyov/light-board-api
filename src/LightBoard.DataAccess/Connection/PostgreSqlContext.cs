using LightBoard.DataAccess.EntityConfigurations;
using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Connection;

public class PostgreSqlContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}