using BeFitApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFitApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ExerciseSession> ExerciseSessions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<MonthlyFee> MonthlyFees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.ExerciseTypeNav)
                .WithMany(et => et.Exercises)
                .HasForeignKey(e => e.ExerciseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.ExerciseSessionNav)
                .WithMany(ts => ts.Exercises)
                .HasForeignKey(e => e.ExerciseSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExerciseSession>()
                .HasOne(es => es.CreatedBy)
                .WithMany()
                .HasForeignKey(es => es.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .Property(e => e.Weight)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MonthlyFee>()
                .Property(f => f.Fee)
                .HasColumnType("decimal(18,2)");
        }
    }
}