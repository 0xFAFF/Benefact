// <auto-generated />
using System;
using BenefactAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BenefactAPI.Migrations
{
    [DbContext(typeof(BenefactDbContext))]
    [Migration("20190521014152_Activity")]
    partial class Activity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("BenefactAPI.DataAccess.ActivityData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("CardId")
                        .HasColumnName("card_id");

                    b.Property<int?>("CommentId")
                        .HasColumnName("comment_id");

                    b.Property<string>("Message")
                        .HasColumnName("message");

                    b.Property<double>("Time")
                        .HasColumnName("time");

                    b.Property<int>("Type")
                        .HasColumnName("type");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_activity");

                    b.HasIndex("UserId")
                        .HasName("ix_activity_user_id");

                    b.HasIndex("BoardId", "CardId")
                        .HasName("ix_activity_board_id_card_id");

                    b.HasIndex("BoardId", "CommentId")
                        .HasName("ix_activity_board_id_comment_id");

                    b.ToTable("activity");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.AttachmentData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("CardId")
                        .HasColumnName("card_id");

                    b.Property<string>("ContentType")
                        .HasColumnName("content_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("Preview")
                        .HasColumnName("preview");

                    b.Property<int?>("StorageId")
                        .HasColumnName("storage_id");

                    b.Property<string>("Url")
                        .HasColumnName("url");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_attachments");

                    b.HasIndex("StorageId")
                        .IsUnique()
                        .HasName("ix_attachments_storage_id");

                    b.HasIndex("UserId")
                        .HasName("ix_attachments_user_id");

                    b.HasIndex("BoardId", "CardId")
                        .HasName("ix_attachments_board_id_card_id");

                    b.ToTable("attachments");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.BoardData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("CreatorId")
                        .HasColumnName("creator_id");

                    b.Property<int?>("DefaultPrivilege")
                        .HasColumnName("default_privilege");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title");

                    b.Property<string>("UrlName")
                        .IsRequired()
                        .HasColumnName("url_name");

                    b.HasKey("Id")
                        .HasName("pk_boards");

                    b.HasIndex("CreatorId")
                        .HasName("ix_boards_creator_id");

                    b.HasIndex("UrlName")
                        .IsUnique()
                        .HasName("ix_boards_url_name");

                    b.ToTable("boards");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.CardData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool?>("Archived")
                        .IsRequired()
                        .HasColumnName("archived");

                    b.Property<int>("AuthorId")
                        .HasColumnName("author_id");

                    b.Property<int?>("ColumnId")
                        .IsRequired()
                        .HasColumnName("column_id");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<int?>("Index")
                        .IsRequired()
                        .HasColumnName("index");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_cards");

                    b.HasIndex("AuthorId")
                        .HasName("ix_cards_author_id");

                    b.HasIndex("BoardId", "ColumnId")
                        .HasName("ix_cards_board_id_column_id");

                    b.ToTable("cards");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.CardTag", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("CardId")
                        .HasColumnName("card_id");

                    b.Property<int>("TagId")
                        .HasColumnName("tag_id");

                    b.HasKey("BoardId", "CardId", "TagId")
                        .HasName("pk_card_tag");

                    b.HasIndex("BoardId", "TagId")
                        .HasName("ix_card_tag_board_id_tag_id");

                    b.ToTable("card_tag");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.ColumnData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("AllowContribution")
                        .HasColumnName("allow_contribution");

                    b.Property<int?>("Index")
                        .HasColumnName("index");

                    b.Property<int>("State")
                        .HasColumnName("state");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_columns");

                    b.ToTable("columns");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.CommentData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("CardId")
                        .HasColumnName("card_id");

                    b.Property<double>("CreatedTime")
                        .HasColumnName("created_time");

                    b.Property<double?>("EditedTime")
                        .HasColumnName("edited_time");

                    b.Property<string>("Text")
                        .HasColumnName("text");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_comments");

                    b.HasIndex("UserId")
                        .HasName("ix_comments_user_id");

                    b.HasIndex("BoardId", "CardId")
                        .HasName("ix_comments_board_id_card_id");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.InviteData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnName("key")
                        .HasMaxLength(10);

                    b.Property<int>("Privilege")
                        .HasColumnName("privilege");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_invites");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasName("ix_invites_key");

                    b.ToTable("invites");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.StorageEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnName("data");

                    b.HasKey("Id")
                        .HasName("pk_files");

                    b.ToTable("files");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.TagData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Character")
                        .HasColumnName("character");

                    b.Property<string>("Color")
                        .HasColumnName("color");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.HasKey("BoardId", "Id")
                        .HasName("pk_tags");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.UserData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.Property<bool>("EmailVerified")
                        .HasColumnName("email_verified");

                    b.Property<string>("Hash")
                        .HasColumnName("hash");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<Guid?>("Nonce")
                        .HasColumnName("nonce");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasAlternateKey("Email")
                        .HasName("ak_users_email");

                    b.ToTable("users");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("Privilege")
                        .HasColumnName("privilege");

                    b.HasKey("UserId", "BoardId")
                        .HasName("pk_roles");

                    b.HasIndex("BoardId")
                        .HasName("ix_roles_board_id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.VoteData", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnName("board_id");

                    b.Property<int>("CardId")
                        .HasColumnName("card_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.Property<int>("Count")
                        .HasColumnName("count");

                    b.HasKey("BoardId", "CardId", "UserId")
                        .HasName("pk_votes");

                    b.HasIndex("UserId")
                        .HasName("ix_votes_user_id");

                    b.ToTable("votes");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.ActivityData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Activity")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_activity_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.UserData", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_activity_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.CardData", "Card")
                        .WithMany("Activity")
                        .HasForeignKey("BoardId", "CardId")
                        .HasConstraintName("fk_activity_cards_board_id_card_id");

                    b.HasOne("BenefactAPI.DataAccess.CommentData", "Comment")
                        .WithMany("Activity")
                        .HasForeignKey("BoardId", "CommentId")
                        .HasConstraintName("fk_activity_comments_board_id_comment_id");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.AttachmentData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Attachments")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_attachments_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.StorageEntry", "Storage")
                        .WithOne("Attachment")
                        .HasForeignKey("BenefactAPI.DataAccess.AttachmentData", "StorageId")
                        .HasConstraintName("fk_attachments_files_storage_id");

                    b.HasOne("BenefactAPI.DataAccess.UserData", "User")
                        .WithMany("Attachments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_attachments_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.CardData", "Card")
                        .WithMany("Attachments")
                        .HasForeignKey("BoardId", "CardId")
                        .HasConstraintName("fk_attachments_cards_board_id_card_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.BoardData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.UserData", "Creator")
                        .WithMany("CreatedBoards")
                        .HasForeignKey("CreatorId")
                        .HasConstraintName("fk_boards_users_creator_id");
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.CardData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.UserData", "Author")
                        .WithMany("CreatedCards")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("fk_cards_users_author_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Cards")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_cards_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.ColumnData", "Column")
                        .WithMany("Cards")
                        .HasForeignKey("BoardId", "ColumnId")
                        .HasConstraintName("fk_cards_columns_board_id_column_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.CardTag", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany()
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_card_tag_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.CardData", "Card")
                        .WithMany("Tags")
                        .HasForeignKey("BoardId", "CardId")
                        .HasConstraintName("fk_card_tag_cards_board_id_card_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.TagData", "Tag")
                        .WithMany("CardTags")
                        .HasForeignKey("BoardId", "TagId")
                        .HasConstraintName("fk_card_tag_tags_board_id_tag_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.ColumnData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Columns")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_columns_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.CommentData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Comments")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_comments_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.UserData", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_comments_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.CardData", "Card")
                        .WithMany("Comments")
                        .HasForeignKey("BoardId", "CardId")
                        .HasConstraintName("fk_comments_cards_board_id_card_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.InviteData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Invites")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_invites_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.TagData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Tags")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_tags_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.UserRole", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Roles")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_roles_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.UserData", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_roles_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BenefactAPI.DataAccess.VoteData", b =>
                {
                    b.HasOne("BenefactAPI.DataAccess.BoardData", "Board")
                        .WithMany("Votes")
                        .HasForeignKey("BoardId")
                        .HasConstraintName("fk_votes_boards_board_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.UserData", "User")
                        .WithMany("Votes")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_votes_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BenefactAPI.DataAccess.CardData", "Card")
                        .WithMany("Votes")
                        .HasForeignKey("BoardId", "CardId")
                        .HasConstraintName("fk_votes_cards_board_id_card_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
