﻿// <auto-generated />
using System;
using LightBoard.DataAccess.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LightBoard.DataAccess.Migrations.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    partial class PostgreSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LightBoard.Domain.Entities.Attachments.CardAttachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid")
                        .HasColumnName("card_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime>("UploadedAtUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("uploaded_at_utc");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_card_attachment");

                    b.HasIndex("CardId")
                        .HasDatabaseName("ix_card_attachment_card_id");

                    b.ToTable("card_attachment", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Auth.CodeBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.Property<string>("ResetCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reset_code");

                    b.HasKey("Id")
                        .HasName("pk_generated_codes");

                    b.ToTable("generated_codes", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("CodeBase");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Auth.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AvatarBlobName")
                        .HasColumnType("text")
                        .HasColumnName("avatar_blob_name");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_email_confirmed");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Boards.Board", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("BoardBackgroundUrl")
                        .HasColumnType("text")
                        .HasColumnName("board_background_url");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean")
                        .HasColumnName("is_archived");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_boards");

                    b.ToTable("boards", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Boards.BoardMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uuid")
                        .HasColumnName("board_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_board_members");

                    b.HasIndex("BoardId")
                        .HasDatabaseName("ix_board_members_board_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_board_members_user_id");

                    b.ToTable("board_members", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Cards.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ColumnId")
                        .HasColumnType("uuid")
                        .HasColumnName("column_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_cards");

                    b.HasIndex("ColumnId")
                        .HasDatabaseName("ix_cards_column_id");

                    b.ToTable("cards", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Cards.CardAssignee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid")
                        .HasColumnName("card_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_card_assignees");

                    b.HasIndex("CardId")
                        .HasDatabaseName("ix_card_assignees_card_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_card_assignees_user_id");

                    b.ToTable("card_assignees", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Columns.Column", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uuid")
                        .HasColumnName("board_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.HasKey("Id")
                        .HasName("pk_columns");

                    b.HasIndex("BoardId")
                        .HasDatabaseName("ix_columns_board_id");

                    b.ToTable("columns", (string)null);
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Auth.ConfirmEmailCode", b =>
                {
                    b.HasBaseType("LightBoard.Domain.Entities.Auth.CodeBase");

                    b.HasDiscriminator().HasValue("ConfirmEmailCode");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Auth.ResetPasswordCode", b =>
                {
                    b.HasBaseType("LightBoard.Domain.Entities.Auth.CodeBase");

                    b.HasDiscriminator().HasValue("ResetPasswordCode");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Attachments.CardAttachment", b =>
                {
                    b.HasOne("LightBoard.Domain.Entities.Cards.Card", "Card")
                        .WithMany("Attachments")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_card_attachment_cards_card_temp_id");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Boards.BoardMember", b =>
                {
                    b.HasOne("LightBoard.Domain.Entities.Boards.Board", "Board")
                        .WithMany("BoardMembers")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_board_members_boards_board_temp_id");

                    b.HasOne("LightBoard.Domain.Entities.Auth.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_board_members_users_user_temp_id");

                    b.Navigation("Board");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Cards.Card", b =>
                {
                    b.HasOne("LightBoard.Domain.Entities.Columns.Column", "Column")
                        .WithMany("Cards")
                        .HasForeignKey("ColumnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cards_columns_column_temp_id");

                    b.Navigation("Column");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Cards.CardAssignee", b =>
                {
                    b.HasOne("LightBoard.Domain.Entities.Cards.Card", "Card")
                        .WithMany("CardAssignees")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_card_assignees_cards_card_temp_id1");

                    b.HasOne("LightBoard.Domain.Entities.Auth.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_card_assignees_users_user_temp_id");

                    b.Navigation("Card");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Columns.Column", b =>
                {
                    b.HasOne("LightBoard.Domain.Entities.Boards.Board", "Board")
                        .WithMany("Columns")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_columns_boards_board_temp_id1");

                    b.Navigation("Board");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Boards.Board", b =>
                {
                    b.Navigation("BoardMembers");

                    b.Navigation("Columns");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Cards.Card", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("CardAssignees");
                });

            modelBuilder.Entity("LightBoard.Domain.Entities.Columns.Column", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
