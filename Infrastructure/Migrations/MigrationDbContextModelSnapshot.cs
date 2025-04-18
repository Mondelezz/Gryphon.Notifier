﻿// <auto-generated />
using System;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MigrationDbContext))]
    partial class MigrationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<DateTime>("DateEvent")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Importance")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsIterative")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<TimeOnly?>("TimeEventEnded")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly?>("TimeEventStart")
                        .HasColumnType("time without time zone");

                    b.Property<long?>("TopicId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Domain.Models.FileData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<string>("FileHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.HasKey("Id");

                    b.ToTable("FileDatas");
                });

            modelBuilder.Entity("Domain.Models.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TopicType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Models.UserLogin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Domain.Models.UserToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("AccessTokenExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', current_timestamp)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserLoginId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserLoginId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Domain.Models.Event", b =>
                {
                    b.HasOne("Domain.Models.Topic", "Topic")
                        .WithMany("Events")
                        .HasForeignKey("TopicId");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Domain.Models.UserLogin", b =>
                {
                    b.HasOne("Domain.Models.User", null)
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.UserToken", b =>
                {
                    b.HasOne("Domain.Models.User", null)
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.UserLogin", "UserLogin")
                        .WithMany()
                        .HasForeignKey("UserLoginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserLogin");
                });

            modelBuilder.Entity("Domain.Models.Topic", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("Domain.Models.User", b =>
                {
                    b.Navigation("UserLogins");

                    b.Navigation("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
