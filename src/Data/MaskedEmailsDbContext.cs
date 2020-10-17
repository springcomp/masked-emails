using System;
using System.Threading.Tasks;
using Data.Interop;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data
{
    // https://docs.microsoft.com/fr-fr/aspnet/core/data/ef-mvc/intro
    // http://www.learnentityframeworkcore.com/configuration/fluent-api/hasone-method

    public sealed class MaskedEmailsDbContext : DbContext, IMaskedEmailsDbContext
    {
        public MaskedEmailsDbContext(DbContextOptions<MaskedEmailsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profile> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        void IBaseDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app")
                ;

            modelBuilder.Entity<Profile>()
                .ToTable("Profiles")
                .HasKey(e => e.Id)
                ;

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.Addresses)
                .WithOne(e => e.Profile)
                .HasForeignKey("Profile_Id")
                ;

            modelBuilder.Entity<Address>()
                .ToTable("Addresses")
                .HasKey(e => e.Id)
                ;

            modelBuilder.Entity<Address>()
                .Property(e => e.EnableForwarding)
                .HasDefaultValue(true)
                ;

            modelBuilder.Entity<Address>()
                .Property(e => e.Received)
                .HasDefaultValue(0)
                ;
        }
    }
}