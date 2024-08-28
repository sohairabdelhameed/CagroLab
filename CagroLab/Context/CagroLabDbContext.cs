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
            // Lab - Accounts Relationship
            modelBuilder.Entity<Lab>()
                .HasMany(l => l.Accounts)
                .WithOne(a => a.Lab)
                .HasForeignKey(a => a.Lab_Id);

            // Lab - Package Relationship
            modelBuilder.Entity<Lab>()
                .HasMany(l => l.Packages)
                .WithOne(a => a.Lab)
                .HasForeignKey(a => a.Lab_Id);

            modelBuilder.Entity<Package>()
            .HasOne(p => p.Account)
            .WithMany(a => a.Packages)
           .HasForeignKey(p => p.Account_Id)
           .OnDelete(DeleteBehavior.Cascade);



            // Package - Samples Relationship
            modelBuilder.Entity<Package>()
                .HasMany(p => p.Samples)
                .WithOne(s => s.Package)
                .HasForeignKey(s => s.Package_Id)
                 .OnDelete(DeleteBehavior.NoAction); ;


            // Account - Samples Relationship
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Samples)
                .WithOne(s => s.Account)
                .HasForeignKey(s => s.Account_Id);
                

            base.OnModelCreating(modelBuilder);
        }






    }
}
