﻿// <auto-generated />
using System;
using CabaVS.ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240606191902_AddWorkspaces")]
    partial class AddWorkspaces
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.UserWorkspace", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "WorkspaceId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("UserWorkspaces");
                });

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.Workspace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.UserWorkspace", b =>
                {
                    b.HasOne("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("UserWorkspaces")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.Workspace", "Workspace")
                        .WithMany("UserWorkspaces")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Navigation("UserWorkspaces");
                });

            modelBuilder.Entity("CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities.Workspace", b =>
                {
                    b.Navigation("UserWorkspaces");
                });
#pragma warning restore 612, 618
        }
    }
}
