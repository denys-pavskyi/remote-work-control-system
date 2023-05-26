﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(RemoteWorkControlSystemDbContext))]
    partial class RemoteWorkControlSystemDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DAL.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsScreenActivityControlEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("JiraDomain")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectKey")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ProjectTitle")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("ScreenshotInterval")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DAL.Entities.ProjectMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectMembers");
                });

            modelBuilder.Entity("DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("JiraApiKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JiraBaseUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "dpavsky@gmail.com",
                            FirstName = "Denys",
                            JiraApiKey = "ATATT3xFfGF0zKVExXVUI7se6r5sZekIGQL9cgiwmLiWCgDXjstSgt48rtJhJvX71geSrJbOdWPz1c8I1tqWvSVWdI_gJfoAxDpS8XJYkF_SZG6wcLpV_Eu8c44v7436cgwvuJ63rjh-Zluy7Svvsrg_e6hRm-a83pg6AMyM47qZ9OGzFpeEUJQ=0CD67135",
                            JiraBaseUrl = "test-rwcs",
                            LastName = "Pavskyi",
                            Password = "password1",
                            UserName = "denys_pavskyi2"
                        },
                        new
                        {
                            Id = 2,
                            Email = "denchik.arasty000@gmail.com",
                            FirstName = "Denis",
                            JiraApiKey = "ATATT3xFfGF04dWC_ws0K9fPjFB1KIZtP4TSisM-yAKQzQEn6hGqwElrEraynNKfFcz6KVx7Kv1dYIML9CdtqdTfhSAcCJHTDzclxOSrRQ3UUP1KFpOAABfKVYvg6qxd9Y3ni9WBDmTkmtVY56fOvebM0cYh-wiHBtjNwI0rSVNK7rQW9wccXig=6019D62C",
                            JiraBaseUrl = "test-rwcs",
                            LastName = "Test",
                            Password = "password1",
                            UserName = "denis_test"
                        });
                });

            modelBuilder.Entity("DAL.Entities.WorkSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProjectMemberId")
                        .HasColumnType("int");

                    b.Property<string>("ScreenActivityFolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SprintKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("WorkTime")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ProjectMemberId");

                    b.ToTable("WorkSessions");
                });

            modelBuilder.Entity("DAL.Entities.ProjectMember", b =>
                {
                    b.HasOne("DAL.Entities.Project", "Project")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DAL.Entities.User", "User")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.Entities.WorkSession", b =>
                {
                    b.HasOne("DAL.Entities.ProjectMember", "ProjectMember")
                        .WithMany("WorkSessions")
                        .HasForeignKey("ProjectMemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ProjectMember");
                });

            modelBuilder.Entity("DAL.Entities.Project", b =>
                {
                    b.Navigation("ProjectMembers");
                });

            modelBuilder.Entity("DAL.Entities.ProjectMember", b =>
                {
                    b.Navigation("WorkSessions");
                });

            modelBuilder.Entity("DAL.Entities.User", b =>
                {
                    b.Navigation("ProjectMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
