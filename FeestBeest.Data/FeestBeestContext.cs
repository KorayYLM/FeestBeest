using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;
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

                entity.HasData(
                    new Beestje { Id = 1, Naam = "Aap", Type = "Jungle", Prijs = 100.00m, Afbeelding = "https://e7.pngegg.com/pngimages/712/791/png-clipart-graphics-drawing-cartoon-monkey-mammal-cat-like-mammal-thumbnail.png" },
                    new Beestje { Id = 2, Naam = "Olifant", Type = "Jungle", Prijs = 300.00m, Afbeelding = "olifant.jpg" },
                    new Beestje { Id = 3, Naam = "Zebra", Type = "Jungle", Prijs = 220.00m, Afbeelding = "zebra.jpg" },
                    new Beestje { Id = 4, Naam = "Leeuw", Type = "Jungle", Prijs = 250.00m, Afbeelding = "leeuw.jpg" },
                    new Beestje { Id = 5, Naam = "Hond", Type = "Boerderij", Prijs = 80.00m, Afbeelding = "hond.jpg" },
                    new Beestje { Id = 6, Naam = "Ezel", Type = "Boerderij", Prijs = 150.00m, Afbeelding = "ezel.jpg" },
                    new Beestje { Id = 7, Naam = "Koe", Type = "Boerderij", Prijs = 180.00m, Afbeelding = "koe.jpg" },
                    new Beestje { Id = 8, Naam = "Eend", Type = "Boerderij", Prijs = 50.00m, Afbeelding = "eend.jpg" },
                    new Beestje { Id = 9, Naam = "Kuiken", Type = "Boerderij", Prijs = 20.00m, Afbeelding = "kuiken.jpg" },
                    new Beestje { Id = 10, Naam = "Pinguïn", Type = "Sneeuw", Prijs = 90.00m, Afbeelding = "pinguin.jpg" },
                    new Beestje { Id = 11, Naam = "IJsbeer", Type = "Sneeuw", Prijs = 200.00m, Afbeelding = "ijsbeer.jpg" },
                    new Beestje { Id = 12, Naam = "Zeehond", Type = "Sneeuw", Prijs = 130.00m, Afbeelding = "zeehond.jpg" },
                    new Beestje { Id = 13, Naam = "Kameel", Type = "Woestijn", Prijs = 160.00m, Afbeelding = "kameel.jpg" },
                    new Beestje { Id = 14, Naam = "Slang", Type = "Woestijn", Prijs = 70.00m, Afbeelding = "slang.jpg" },
                    new Beestje { Id = 15, Naam = "T-Rex", Type = "VIP", Prijs = 1000.00m, Afbeelding = "trex.jpg" },
                    new Beestje { Id = 16, Naam = "Unicorn", Type = "VIP", Prijs = 1500.00m, Afbeelding = "unicorn.jpg" },
                    new Beestje { Id = 17, Naam = "Papegaai", Type = "Tropisch", Prijs = 120.00m, Afbeelding = "papegaai.jpg" },
                    new Beestje { Id = 18, Naam = "Krokodil", Type = "Tropisch", Prijs = 200.00m, Afbeelding = "krokodil.jpg" },
                    new Beestje { Id = 19, Naam = "Kangoeroe", Type = "Australisch", Prijs = 180.00m, Afbeelding = "kangoeroe.jpg" },
                    new Beestje { Id = 20, Naam = "Koala", Type = "Australisch", Prijs = 150.00m, Afbeelding = "koala.jpg" }
                );
            });

            var adminRoleId = "1";
            var userRoleId = "2";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            var hasher = new PasswordHasher<Account>();
            var adminId = "1";
            var userId = "2";

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = adminId,
                    UserName = "Boerderijadmin",
                    NormalizedUserName = "BOERDERIJADMIN",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "AdminPassword123"),
                    SecurityStamp = string.Empty,
                    Naam = "Caron Schilders" 
                },
                new Account
                {
                    Id = userId,
                    UserName = "KorayYilmaz",
                    NormalizedUserName = "KORAYYILMAZ",
                    Email = "koray@example.com",
                    NormalizedEmail = "KORAY@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "UserPassword123"),
                    SecurityStamp = string.Empty,
                    Naam = "Koray Yilmaz" 
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminId },
                new IdentityUserRole<string> { RoleId = userRoleId, UserId = userId }
            );
        }
    }
}