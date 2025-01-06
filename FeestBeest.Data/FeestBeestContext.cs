
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FeestBeest.Data
{
    public class FeestBeestContext : IdentityDbContext<Account,IdentityRole<int>,int>
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

                entity.HasData(
                    new Beestje { Id = 1, Naam = "Aap", Type = "Jungle", Prijs = 100.00m, Afbeelding = "monkey.png"}, 
                    new Beestje { Id = 2, Naam = "Olifant", Type = "Jungle", Prijs = 300.00m, Afbeelding = "elephant.png" },
                    new Beestje { Id = 3, Naam = "Zebra", Type = "Jungle", Prijs = 220.00m, Afbeelding = "zebra.png" },
                    new Beestje { Id = 4, Naam = "Leeuw", Type = "Jungle", Prijs = 250.00m, Afbeelding = "lion.png" },
                    new Beestje { Id = 5, Naam = "Hond", Type = "Boerderij", Prijs = 80.00m, Afbeelding = "dog.png" },
                    new Beestje { Id = 6, Naam = "Ezel", Type = "Boerderij", Prijs = 150.00m, Afbeelding = "donkey.png" },
                    new Beestje { Id = 7, Naam = "Koe", Type = "Boerderij", Prijs = 180.00m, Afbeelding = "cow.png" },
                    new Beestje { Id = 8, Naam = "Eend", Type = "Boerderij", Prijs = 50.00m, Afbeelding = "duck.png" },
                    new Beestje { Id = 9, Naam = "Kuiken", Type = "Boerderij", Prijs = 20.00m, Afbeelding = "chicken.png" },
                    new Beestje { Id = 10, Naam = "Pinguïn", Type = "Sneeuw", Prijs = 90.00m, Afbeelding = "animal.png" },
                    new Beestje { Id = 11, Naam = "IJsbeer", Type = "Sneeuw", Prijs = 200.00m, Afbeelding = "polar-bear.png" },
                    new Beestje { Id = 12, Naam = "Zeehond", Type = "Sneeuw", Prijs = 130.00m, Afbeelding = "seal.png" },
                    new Beestje { Id = 13, Naam = "Kameel", Type = "Woestijn", Prijs = 160.00m, Afbeelding = "camel.png" },
                    new Beestje { Id = 14, Naam = "Slang", Type = "Woestijn", Prijs = 70.00m, Afbeelding = "snake.png" },
                    new Beestje { Id = 15, Naam = "T-Rex", Type = "VIP", Prijs = 1000.00m, Afbeelding = "dinosaur.png" },
                    new Beestje { Id = 16, Naam = "Unicorn", Type = "VIP", Prijs = 1500.00m, Afbeelding = "unicorn.png" }
                );
            });

     

            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" }
            );

            var hasher = new PasswordHasher<Account>();

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    UserName = "admin@example.com",
                    NormalizedUserName = "admin@example.com",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "AdminPassword123"),
                    SecurityStamp = string.Empty,
                    Naam = "Caron Schilders"
                },
                new Account
                {
                    Id = 2,
                    UserName = "koray@example.com",
                    NormalizedUserName = "koray@example.com",
                    Email = "koray@example.com",
                    NormalizedEmail = "KORAY@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "UserPassword123"),
                    SecurityStamp = string.Empty,
                    Naam = "Koray Yilmaz"
                }
            );
            
        }
    }
}