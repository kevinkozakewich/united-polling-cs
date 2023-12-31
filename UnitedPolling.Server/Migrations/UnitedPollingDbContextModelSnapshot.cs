﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnitedPolling.DataContext;

#nullable disable

namespace UnitedPolling.Server.Migrations
{
    [DbContext(typeof(UnitedPollingDbContext))]
    partial class UnitedPollingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UnitedPolling.DataContext.Models.Poll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<bool>("UrlShareable")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Poll");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollAdministrator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.HasIndex("UserId");

                    b.ToTable("PollAdministrators");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CompletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.HasIndex("UserId");

                    b.ToTable("PollParticipants");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)");

                    b.Property<int>("QuestionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PollId");

                    b.ToTable("PollQuestion");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestionOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PollQuestionId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("Id");

                    b.HasIndex("PollQuestionId");

                    b.ToTable("PollQuestionOption");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestionResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PollQuestionOptionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("WrittenResponse")
                        .HasColumnType("VARCHAR(500)");

                    b.HasKey("Id");

                    b.HasIndex("PollQuestionOptionId");

                    b.HasIndex("UserId");

                    b.ToTable("PollQuestionResponse");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AspId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLoggedIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.Poll", b =>
                {
                    b.HasOne("UnitedPolling.DataContext.Models.User", "CreatedUser")
                        .WithMany("CreatedUsers")
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("UnitedPolling.DataContext.Models.User", "UpdatedUser")
                        .WithMany("UpdatedUsers")
                        .HasForeignKey("UpdatedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("UpdatedUser");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollAdministrator", b =>
                {
                    b.HasOne("UnitedPolling.DataContext.Models.Poll", "Poll")
                        .WithMany("PollAdministrators")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UnitedPolling.DataContext.Models.User", "User")
                        .WithMany("PollAdministrators")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poll");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollParticipant", b =>
                {
                    b.HasOne("UnitedPolling.DataContext.Models.Poll", "Poll")
                        .WithMany("PollParticipants")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UnitedPolling.DataContext.Models.User", "User")
                        .WithMany("PollParticipants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poll");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestion", b =>
                {
                    b.HasOne("UnitedPolling.DataContext.Models.Poll", null)
                        .WithMany("PollQuestions")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestionOption", b =>
                {
                    b.HasOne("UnitedPolling.DataContext.Models.PollQuestion", null)
                        .WithMany("PollQuestionOptions")
                        .HasForeignKey("PollQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestionResponse", b =>
                {
                    b.HasOne("UnitedPolling.DataContext.Models.PollQuestionOption", "PollQuestionOption")
                        .WithMany("PollQuestionResponse")
                        .HasForeignKey("PollQuestionOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UnitedPolling.DataContext.Models.User", "User")
                        .WithMany("PollQuestionReponse")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PollQuestionOption");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.Poll", b =>
                {
                    b.Navigation("PollAdministrators");

                    b.Navigation("PollParticipants");

                    b.Navigation("PollQuestions");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestion", b =>
                {
                    b.Navigation("PollQuestionOptions");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.PollQuestionOption", b =>
                {
                    b.Navigation("PollQuestionResponse");
                });

            modelBuilder.Entity("UnitedPolling.DataContext.Models.User", b =>
                {
                    b.Navigation("CreatedUsers");

                    b.Navigation("PollAdministrators");

                    b.Navigation("PollParticipants");

                    b.Navigation("PollQuestionReponse");

                    b.Navigation("UpdatedUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
