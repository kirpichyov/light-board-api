﻿using LightBoard.DataAccess.EntityConfigurations;
using LightBoard.Domain.Entities.Attachments;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Domain.Entities.Boards;
using LightBoard.Domain.Entities.Cards;
using LightBoard.Domain.Entities.Columns;
using LightBoard.Domain.Entities.Record;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Connection;

public class PostgreSqlContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<CodeBase> GeneratedCodes { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardMember> BoardMembers { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<ActionHistoryRecord> ActionHistoryRecords { get; set; }
    public DbSet<CardAssignee> CardAssignees { get; set; }
    public DbSet<CardAttachment> CardAttachment { get; set; }
    public DbSet<CardComment> CardComments { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

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