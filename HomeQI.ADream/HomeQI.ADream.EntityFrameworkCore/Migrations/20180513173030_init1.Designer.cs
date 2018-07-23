﻿// <auto-generated />
using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;

namespace HomeQI.ADream.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ADreamDbContext))]
    [Migration("20180513173030_init1")]
    partial class init1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<string>("AreaName");

                    b.Property<string>("Controller")
                        .IsRequired();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsAllowAnonymous");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEnable");

                    b.Property<int>("Level");

                    b.Property<string>("Params");

                    b.Property<int>("ShowSort");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId");

                    b.Property<string>("UserRoleId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserRoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.RoleClaim", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEnable");

                    b.Property<string>("PermissionId");

                    b.Property<string>("Regular");

                    b.Property<string>("RoleId");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTime?>("LockoutEnd");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(50);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityAudit");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Sex");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserClaimId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("UserClaimId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserClaim", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEnable");

                    b.Property<string>("PermissionId");

                    b.Property<string>("Regular");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserLogin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("ProviderKey");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<DateTime>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Microsoft.EntityFrameworkCore.AutoHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Changed")
                        .HasMaxLength(2048);

                    b.Property<DateTime>("Created");

                    b.Property<int>("Kind");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("AutoHistory");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.Permission", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.Role", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");

                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.UserRole")
                        .WithMany("Roles")
                        .HasForeignKey("UserRoleId");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.RoleClaim", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.User", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.UserClaim")
                        .WithMany("User")
                        .HasForeignKey("UserClaimId");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserLogin", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserRole", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HomeQI.ADream.Models.Entities.Identity.UserToken", b =>
                {
                    b.HasOne("HomeQI.ADream.Models.Entities.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
