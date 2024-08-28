using CagroLab.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CagroLab.Context
{
    public class CagroLabDbContext : DbContext
    {
        public CagroLabDbContext(DbContextOptions<CagroLabDbContext> options)
        : base(options)
        {
        }

        public DbSet<Lab> Lab { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Sample> Sample { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lab>()
       .HasQueryFilter(l => !l.IsDeleted);

            modelBuilder.Entity<Account>()
                .HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<Package>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<Sample>()
                .HasQueryFilter(s => !s.IsDeleted);

            // Lab - Accounts Relationship
            modelBuilder.Entity<Lab>()
                .HasMany(l => l.Accounts)
                .WithOne(a => a.Lab)
                .HasForeignKey(a => a.Lab_Id)
                .OnDelete(DeleteBehavior.ClientSetNull); // Cascade delete for Lab -> Accounts

            // Lab - Package Relationship
            modelBuilder.Entity<Lab>()
                .HasMany(l => l.Packages)
                .WithOne(a => a.Lab)
                .HasForeignKey(a => a.Lab_Id)
                .OnDelete(DeleteBehavior.ClientSetNull); // Cascade delete for Lab -> Packages

            // Package - Account Relationship
            modelBuilder.Entity<Package>()
                .HasOne(p => p.Account)
                .WithMany(a => a.Packages)
                .HasForeignKey(p => p.Account_Id)
                .OnDelete(DeleteBehavior.ClientSetNull); // Cascade delete for Package -> Account

            // Package - Samples Relationship
            modelBuilder.Entity<Package>()
                .HasMany(p => p.Samples)
                .WithOne(s => s.Package)
                .HasForeignKey(s => s.Package_Id)
                .OnDelete(DeleteBehavior.ClientSetNull); // Cascade delete for Package -> Samples

            // Account - Samples Relationship
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Samples)
                .WithOne(s => s.Account)
                .HasForeignKey(s => s.Account_Id)
                .OnDelete(DeleteBehavior.ClientSetNull); // Cascade delete for Account -> Samples

            base.OnModelCreating(modelBuilder);
        }
    }
}
