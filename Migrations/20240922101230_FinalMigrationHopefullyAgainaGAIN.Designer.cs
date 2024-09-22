﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BackendApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendApp.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20240922101230_FinalMigrationHopefullyAgainaGAIN")]
    partial class FinalMigrationHopefullyAgainaGAIN
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityAlwaysColumns(modelBuilder);

            modelBuilder.Entity("BackendApp.Model.AdminUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("BackendApp.Model.Connection", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<bool>("Accepted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UsersReceivedNotificationId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersSentNotificationId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UsersReceivedNotificationId");

                    b.HasIndex("UsersSentNotificationId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("BackendApp.Model.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("ReceivedId")
                        .HasColumnType("bigint");

                    b.Property<long>("SentId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ReceivedId");

                    b.HasIndex("SentId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BackendApp.Model.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("NotificIds")
                        .HasColumnType("bigint");

                    b.Property<long>("NotificationsIds")
                        .HasColumnType("bigint");

                    b.Property<bool>("Read")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("NotificIds");

                    b.HasIndex("NotificationsIds");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BackendApp.Model.PostBase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<DateTime>("PostedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("PostedById")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PostedById");

                    b.ToTable("PostBase");

                    b.HasDiscriminator().HasValue("PostBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BackendApp.Model.PostFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PostFile");
                });

            modelBuilder.Entity("BackendApp.Model.RegularUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("RegularUsers");
                });

            modelBuilder.Entity("BackendApp.Model.RegularUserHideableInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<List<string>>("Capabilities")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<bool>("CapabilitiesArePublic")
                        .HasColumnType("boolean");

                    b.Property<string>("CurrentPosition")
                        .HasColumnType("text");

                    b.Property<bool>("CurrentPositionIsPublic")
                        .HasColumnType("boolean");

                    b.Property<List<string>>("Education")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<bool>("EducationIsPublic")
                        .HasColumnType("boolean");

                    b.Property<List<string>>("Experience")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<bool>("ExperienceIsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LocationIsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberIsPublic")
                        .HasColumnType("boolean");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RegularUserHideableInfo");
                });

            modelBuilder.Entity("PostBasePostFile", b =>
                {
                    b.Property<long>("PostFilesId")
                        .HasColumnType("bigint");

                    b.Property<long>("PostsUsedInId")
                        .HasColumnType("bigint");

                    b.HasKey("PostFilesId", "PostsUsedInId");

                    b.HasIndex("PostsUsedInId");

                    b.ToTable("PostBasePostFile");
                });

            modelBuilder.Entity("PostBaseRegularUser", b =>
                {
                    b.Property<long>("InterestedUsersId")
                        .HasColumnType("bigint");

                    b.Property<long>("LikedPostsId")
                        .HasColumnType("bigint");

                    b.HasKey("InterestedUsersId", "LikedPostsId");

                    b.HasIndex("LikedPostsId");

                    b.ToTable("PostBaseRegularUser");
                });

            modelBuilder.Entity("BackendApp.Model.JobPost", b =>
                {
                    b.HasBaseType("BackendApp.Model.PostBase");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("Requirements")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasDiscriminator().HasValue("JobPost");
                });

            modelBuilder.Entity("BackendApp.Model.Post", b =>
                {
                    b.HasBaseType("BackendApp.Model.PostBase");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsReply")
                        .HasColumnType("boolean");

                    b.Property<long?>("OriginalPost")
                        .HasColumnType("bigint");

                    b.HasIndex("OriginalPost");

                    b.HasDiscriminator().HasValue("Post");
                });

            modelBuilder.Entity("BackendApp.Model.Connection", b =>
                {
                    b.HasOne("BackendApp.Model.RegularUser", "SentTo")
                        .WithMany()
                        .HasForeignKey("UsersReceivedNotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendApp.Model.RegularUser", "SentBy")
                        .WithMany()
                        .HasForeignKey("UsersSentNotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SentBy");

                    b.Navigation("SentTo");
                });

            modelBuilder.Entity("BackendApp.Model.Message", b =>
                {
                    b.HasOne("BackendApp.Model.RegularUser", "SentTo")
                        .WithMany()
                        .HasForeignKey("ReceivedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendApp.Model.RegularUser", "SentBy")
                        .WithMany()
                        .HasForeignKey("SentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SentBy");

                    b.Navigation("SentTo");
                });

            modelBuilder.Entity("BackendApp.Model.Notification", b =>
                {
                    b.HasOne("BackendApp.Model.PostBase", "AssociatedPost")
                        .WithMany()
                        .HasForeignKey("NotificIds");

                    b.HasOne("BackendApp.Model.RegularUser", "ToUser")
                        .WithMany()
                        .HasForeignKey("NotificationsIds")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssociatedPost");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("BackendApp.Model.PostBase", b =>
                {
                    b.HasOne("BackendApp.Model.RegularUser", "PostedBy")
                        .WithMany()
                        .HasForeignKey("PostedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostedBy");
                });

            modelBuilder.Entity("BackendApp.Model.RegularUserHideableInfo", b =>
                {
                    b.HasOne("BackendApp.Model.RegularUser", null)
                        .WithOne("HideableInfo")
                        .HasForeignKey("BackendApp.Model.RegularUserHideableInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostBasePostFile", b =>
                {
                    b.HasOne("BackendApp.Model.PostFile", null)
                        .WithMany()
                        .HasForeignKey("PostFilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendApp.Model.PostBase", null)
                        .WithMany()
                        .HasForeignKey("PostsUsedInId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostBaseRegularUser", b =>
                {
                    b.HasOne("BackendApp.Model.RegularUser", null)
                        .WithMany()
                        .HasForeignKey("InterestedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendApp.Model.PostBase", null)
                        .WithMany()
                        .HasForeignKey("LikedPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BackendApp.Model.Post", b =>
                {
                    b.HasOne("BackendApp.Model.Post", null)
                        .WithMany("Replies")
                        .HasForeignKey("OriginalPost");
                });

            modelBuilder.Entity("BackendApp.Model.RegularUser", b =>
                {
                    b.Navigation("HideableInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("BackendApp.Model.Post", b =>
                {
                    b.Navigation("Replies");
                });
#pragma warning restore 612, 618
        }
    }
}
