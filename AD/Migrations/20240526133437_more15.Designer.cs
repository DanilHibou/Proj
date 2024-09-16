﻿// <auto-generated />
using System;
using AD.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AD.Migrations
{
    [DbContext(typeof(ADContext))]
    [Migration("20240526133437_more15")]
    partial class more15
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AD.Models.ADDomain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DomainControllerAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DomainName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LdapFormat")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DomainController");
                });

            modelBuilder.Entity("AD.Models.ADGroups", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("workerType")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("ADGroups");
                });

            modelBuilder.Entity("AD.Models.ADOU", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("OUName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OUPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("ADOU");
                });

            modelBuilder.Entity("AD.Models.AllowedUsers", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("AllowedUsers");
                });

            modelBuilder.Entity("AD.Models.GoogleDomain", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("GoogleDomain");
                });

            modelBuilder.Entity("AD.Models.GoogleGroups", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Groupid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("workerType")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("GoogleGroups");
                });

            modelBuilder.Entity("AD.Models.GoogleOU", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("OUName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OUPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OUid")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("GoogleOU");
                });

            modelBuilder.Entity("AD.Models.Location", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("AD.Models.UserAccountNames", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserName")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool?>("isADCreated")
                        .HasColumnType("bit");

                    b.Property<bool?>("isGoogleCreated")
                        .HasColumnType("bit");

                    b.Property<int?>("workerSubdivisionid")
                        .HasColumnType("int");

                    b.Property<int>("workerType")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("workerSubdivisionid");

                    b.ToTable("UserAccountNames");
                });

            modelBuilder.Entity("AD.Models.WorkerSubdivision", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("ADOUid")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GoogleOUid")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("typeName")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("ADOUid");

                    b.HasIndex("GoogleOUid");

                    b.ToTable("workerSubdivisions");
                });

            modelBuilder.Entity("ADGroupsUserAccountNames", b =>
                {
                    b.Property<int>("ADGroupsid")
                        .HasColumnType("int");

                    b.Property<int>("UserAccountNamesid")
                        .HasColumnType("int");

                    b.HasKey("ADGroupsid", "UserAccountNamesid");

                    b.HasIndex("UserAccountNamesid");

                    b.ToTable("ADGroupsUserAccountNames");
                });

            modelBuilder.Entity("GoogleGroupsUserAccountNames", b =>
                {
                    b.Property<int>("GoogleGroupsid")
                        .HasColumnType("int");

                    b.Property<int>("UserAccountNamesid")
                        .HasColumnType("int");

                    b.HasKey("GoogleGroupsid", "UserAccountNamesid");

                    b.HasIndex("UserAccountNamesid");

                    b.ToTable("GoogleGroupsUserAccountNames");
                });

            modelBuilder.Entity("LocationUserAccountNames", b =>
                {
                    b.Property<int>("UserAccountNamesid")
                        .HasColumnType("int");

                    b.Property<int>("locationid")
                        .HasColumnType("int");

                    b.HasKey("UserAccountNamesid", "locationid");

                    b.HasIndex("locationid");

                    b.ToTable("LocationUserAccountNames");
                });

            modelBuilder.Entity("AD.Models.UserAccountNames", b =>
                {
                    b.HasOne("AD.Models.WorkerSubdivision", "workerSubdivision")
                        .WithMany("accounts")
                        .HasForeignKey("workerSubdivisionid");

                    b.Navigation("workerSubdivision");
                });

            modelBuilder.Entity("AD.Models.WorkerSubdivision", b =>
                {
                    b.HasOne("AD.Models.ADOU", "ADOU")
                        .WithMany()
                        .HasForeignKey("ADOUid");

                    b.HasOne("AD.Models.GoogleOU", "GoogleOU")
                        .WithMany()
                        .HasForeignKey("GoogleOUid");

                    b.Navigation("ADOU");

                    b.Navigation("GoogleOU");
                });

            modelBuilder.Entity("ADGroupsUserAccountNames", b =>
                {
                    b.HasOne("AD.Models.ADGroups", null)
                        .WithMany()
                        .HasForeignKey("ADGroupsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AD.Models.UserAccountNames", null)
                        .WithMany()
                        .HasForeignKey("UserAccountNamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GoogleGroupsUserAccountNames", b =>
                {
                    b.HasOne("AD.Models.GoogleGroups", null)
                        .WithMany()
                        .HasForeignKey("GoogleGroupsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AD.Models.UserAccountNames", null)
                        .WithMany()
                        .HasForeignKey("UserAccountNamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LocationUserAccountNames", b =>
                {
                    b.HasOne("AD.Models.UserAccountNames", null)
                        .WithMany()
                        .HasForeignKey("UserAccountNamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AD.Models.Location", null)
                        .WithMany()
                        .HasForeignKey("locationid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AD.Models.WorkerSubdivision", b =>
                {
                    b.Navigation("accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
