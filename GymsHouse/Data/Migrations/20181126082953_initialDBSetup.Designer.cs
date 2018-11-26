﻿// <auto-generated />
using GymsHouse.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace GymsHouse.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181126082953_initialDBSetup")]
    partial class initialDBSetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GymsHouse.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("LockoutReason");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Picture_1");

                    b.Property<string>("Picture_2");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UnLockReason");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GymsHouse.Models.BusinessHours", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CenterId")
                        .IsRequired();

                    b.Property<string>("DaysOfWeek")
                        .IsRequired();

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.Property<bool>("IsClosed");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.HasKey("ID");

                    b.HasIndex("CenterId");

                    b.ToTable("BusinessHours");
                });

            modelBuilder.Entity("GymsHouse.Models.Center", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Picture_1");

                    b.Property<string>("Picture_2");

                    b.Property<string>("Picture_3");

                    b.Property<string>("Picture_4");

                    b.HasKey("ID");

                    b.ToTable("Center");
                });

            modelBuilder.Entity("GymsHouse.Models.Coupon", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CouponFor")
                        .IsRequired();

                    b.Property<string>("CouponType")
                        .IsRequired();

                    b.Property<double>("Discount");

                    b.Property<bool>("IsActive");

                    b.Property<double>("MinimumAmount");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Picture");

                    b.HasKey("Id");

                    b.ToTable("Coupon");
                });

            modelBuilder.Entity("GymsHouse.Models.GymsClass", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<double>("Duration");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Picture_1");

                    b.Property<string>("Picture_2");

                    b.Property<string>("Picture_3");

                    b.Property<string>("Picture_4");

                    b.Property<double>("Price");

                    b.HasKey("ID");

                    b.ToTable("GymsClass");
                });

            modelBuilder.Entity("GymsHouse.Models.Instructor", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId")
                        .IsRequired();

                    b.Property<string>("Awards");

                    b.Property<string>("Experiences")
                        .IsRequired();

                    b.Property<string>("History");

                    b.Property<string>("Picture_1");

                    b.Property<string>("Picture_2");

                    b.Property<string>("Picture_3");

                    b.HasKey("ID");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Instructor");
                });

            modelBuilder.Entity("GymsHouse.Models.Location", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CenterId")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Picture_1");

                    b.Property<string>("Picture_2");

                    b.Property<string>("Picture_3");

                    b.Property<string>("Picture_4");

                    b.HasKey("ID");

                    b.HasIndex("CenterId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("GymsHouse.Models.Major", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Major");
                });

            modelBuilder.Entity("GymsHouse.Models.MajorOfInstructor", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("InstructorId")
                        .IsRequired();

                    b.Property<string>("MajorId")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("InstructorId");

                    b.HasIndex("MajorId");

                    b.ToTable("MajorOfInstructor");
                });

            modelBuilder.Entity("GymsHouse.Models.MembersInClass", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId")
                        .IsRequired();

                    b.Property<string>("Comments");

                    b.Property<int>("Rating");

                    b.Property<string>("ScheduleHeaderId")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ScheduleHeaderId");

                    b.ToTable("MembersInClass");
                });

            modelBuilder.Entity("GymsHouse.Models.ScheduleDetails", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.Property<string>("ScheduleHeaderId")
                        .IsRequired();

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.HasKey("ID");

                    b.HasIndex("ScheduleHeaderId");

                    b.ToTable("ScheduleDetails");
                });

            modelBuilder.Entity("GymsHouse.Models.ScheduleHeader", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassId")
                        .IsRequired();

                    b.Property<int>("ClassSize");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("InstructorId")
                        .IsRequired();

                    b.Property<string>("LocationId")
                        .IsRequired();

                    b.Property<string>("Notes");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("ID");

                    b.HasIndex("ClassId");

                    b.HasIndex("InstructorId");

                    b.HasIndex("LocationId");

                    b.ToTable("ScheduleHeader");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GymsHouse.Models.BusinessHours", b =>
                {
                    b.HasOne("GymsHouse.Models.Center", "Center")
                        .WithMany("BusinessHours")
                        .HasForeignKey("CenterId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GymsHouse.Models.Instructor", b =>
                {
                    b.HasOne("GymsHouse.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Instructors")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GymsHouse.Models.Location", b =>
                {
                    b.HasOne("GymsHouse.Models.Center", "Center")
                        .WithMany("Locations")
                        .HasForeignKey("CenterId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GymsHouse.Models.MajorOfInstructor", b =>
                {
                    b.HasOne("GymsHouse.Models.Instructor", "Instructor")
                        .WithMany("MajorsOfInstructors")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GymsHouse.Models.Major", "Major")
                        .WithMany("MajorsOfInstructors")
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GymsHouse.Models.MembersInClass", b =>
                {
                    b.HasOne("GymsHouse.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("MembersInClasses")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GymsHouse.Models.ScheduleHeader", "ScheduleHeader")
                        .WithMany("MembersInClasses")
                        .HasForeignKey("ScheduleHeaderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GymsHouse.Models.ScheduleDetails", b =>
                {
                    b.HasOne("GymsHouse.Models.ScheduleHeader", "ScheduleHeader")
                        .WithMany("ScheduleDetails")
                        .HasForeignKey("ScheduleHeaderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GymsHouse.Models.ScheduleHeader", b =>
                {
                    b.HasOne("GymsHouse.Models.GymsClass", "GymsClass")
                        .WithMany("ScheduleHeaders")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GymsHouse.Models.Instructor", "Instructor")
                        .WithMany("ScheduleHeaders")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GymsHouse.Models.Location", "Location")
                        .WithMany("ScheduleHeaders")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GymsHouse.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GymsHouse.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GymsHouse.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GymsHouse.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
