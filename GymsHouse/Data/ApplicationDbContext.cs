using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GymsHouse.Models;

namespace GymsHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Center> Center { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<GymsClass> GymsClass { get; set; }
        public DbSet<ScheduleHeader> ScheduleHeader { get; set; }
        public DbSet<ScheduleDetails> ScheduleDetails { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<MembersInClass> MembersInClass { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Major> Major { get; set; }
        public DbSet<MajorOfInstructor> MajorOfInstructor { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Location>()
               .HasOne<Center>(p => p.Center)
               .WithMany(p => p.Locations)
               .HasForeignKey(p => p.CenterId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BusinessHours>()
               .HasOne<Center>(p => p.Center)
               .WithMany(p => p.BusinessHours)
               .HasForeignKey(p => p.CenterId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Instructor>()
               .HasOne<ApplicationUser>(p => p.ApplicationUser)
               .WithMany(p => p.Instructors)
               .HasForeignKey(p => p.ApplicationUserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MajorOfInstructor>()
               .HasOne<Instructor>(p => p.Instructor)
               .WithMany(p => p.MajorsOfInstructors)
               .HasForeignKey(p => p.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MajorOfInstructor>()
               .HasOne<Major>(p => p.Major)
               .WithMany(p => p.MajorsOfInstructors)
               .HasForeignKey(p => p.MajorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ScheduleDetails>()
               .HasOne<ScheduleHeader>(p => p.ScheduleHeader)
               .WithMany(p => p.ScheduleDetails)
               .HasForeignKey(p => p.ScheduleHeaderId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ScheduleHeader>()
                .HasOne<GymsClass>(p => p.GymsClass)
                .WithMany(p => p.ScheduleHeaders)
                .HasForeignKey(p => p.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ScheduleHeader>()
                .HasOne<Location>(p => p.Location)
                .WithMany(p => p.ScheduleHeaders)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ScheduleHeader>()
                .HasOne<Instructor>(p => p.Instructor)
                .WithMany(p => p.ScheduleHeaders)
                .HasForeignKey(p => p.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MembersInClass>()
                .HasOne<ApplicationUser>(p => p.ApplicationUser)
                .WithMany(p => p.MembersInClasses)
                .HasForeignKey(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MembersInClass>()
                .HasOne<ScheduleHeader>(p => p.ScheduleHeader)
                .WithMany(p => p.MembersInClasses)
                .HasForeignKey(p => p.ScheduleHeaderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
