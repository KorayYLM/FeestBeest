using FeestBeest.Data.Models; // Add this line to reference the namespace
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FeestBeest.Data
{
    public class FeestBeestContext : IdentityDbContext<Account>
    {
        public FeestBeestContext(DbContextOptions<FeestBeestContext> options)
            : base(options)
        {
        }

        public DbSet<Boeking> Boekingen { get; set; }
        public DbSet<Beestje> Beestjes { get; set; }
        public DbSet<Account> Accounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Boeking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ContactNaam).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactAdres).HasMaxLength(200);
                entity.Property(e => e.ContactEmail).HasMaxLength(100);
                entity.Property(e => e.ContactTelefoonnummer).HasMaxLength(15);
                entity.Property(e => e.TotaalPrijs).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Account)
                    .WithMany(a => a.Boekingen)
                    .HasForeignKey(e => e.AccountId);

                entity.HasOne(e => e.Beestje)
                    .WithMany(b => b.Boekingen)
                    .HasForeignKey(e => e.BeestjeId);
            });

            modelBuilder.Entity<Beestje>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Naam).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Afbeelding).IsRequired();
                entity.Property(e => e.Prijs).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Type).IsRequired();
            });
        }
    }
}