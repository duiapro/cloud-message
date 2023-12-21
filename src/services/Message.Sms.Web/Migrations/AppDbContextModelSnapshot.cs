﻿// <auto-generated />
using System;
using Message.Sms.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Message.Sms.Web.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.ApiServiceProvider", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Authority")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("KeyId");

                    b.ToTable("api_serviceprovider");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.Channel", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ApiServiceProviderId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("CostPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("KeyId");

                    b.HasIndex("ApiServiceProviderId");

                    b.ToTable("channel");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.RechargeCard", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("Code")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("KeyId");

                    b.ToTable("recharge_card");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.Users", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Discount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsVip")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PassWork")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserMobile")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("KeyId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.UsersRechargeLogs", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("AfterBalance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("BeforeBalance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("Code")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UsersRechargeKeyId")
                        .HasColumnType("char(36)");

                    b.HasKey("KeyId");

                    b.ToTable("users_recharge_logs");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.UsersSmsCodeLogs", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApiServiceProviderType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Context")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("KeyId");

                    b.HasIndex("UserId");

                    b.ToTable("users_usemobile_codelogs");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.UsersUseMobileHistory", b =>
                {
                    b.Property<Guid>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApiServiceProviderType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ChannelName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("KeyId");

                    b.ToTable("users_usemobile_history");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.Channel", b =>
                {
                    b.HasOne("Message.Sms.Web.Repositories.Entity.ApiServiceProvider", "ApiServiceProvider")
                        .WithMany("Channels")
                        .HasForeignKey("ApiServiceProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApiServiceProvider");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.UsersSmsCodeLogs", b =>
                {
                    b.HasOne("Message.Sms.Web.Repositories.Entity.Users", "Users")
                        .WithMany("CodeLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.ApiServiceProvider", b =>
                {
                    b.Navigation("Channels");
                });

            modelBuilder.Entity("Message.Sms.Web.Repositories.Entity.Users", b =>
                {
                    b.Navigation("CodeLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
