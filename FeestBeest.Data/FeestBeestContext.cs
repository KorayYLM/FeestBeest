using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FeestBeest.Data
{
    public class FeestBeestContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FeestBeestContext(DbContextOptions<FeestBeestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(u => u.Rank)
                .HasConversion<string>();

            CreateRelations(builder);
            SeedData(builder);
        }

        private static void CreateRelations(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne()
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });
            

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            
            
        }

        private static void SeedData(ModelBuilder builder)
        {
            var passwordHasher = new PasswordHasher<User>();

            var user1 = new User
            {
                Id = 1,
                Rank = Rank.NONE,
                UserName = "Carron",
                NormalizedUserName = "Carron",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PhoneNumber = "0612345678",
                HouseNumber = "123",
                ZipCode = "1234AB",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAEPS3HVgGwreh2VogbGYNNcFZeVOJgO8bLRs+04f5Iucpgy+P86IRXTI4/1xQcPFG2w=="
            };

            var user2 = new User
            {
                Id = 2,
                Rank = Rank.NONE,
                UserName = "TestUser",
                NormalizedUserName = "TestUser",
                Email = "customer@example.com",
                NormalizedEmail = "CUSTOMER@EXAMPLE.COM",
                EmailConfirmed = true,
                PhoneNumber = "0612345678",
                HouseNumber = "123",
                ZipCode = "1234AB",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAEPS3HVgGwreh2VogbGYNNcFZeVOJgO8bLRs+04f5Iucpgy+P86IRXTI4/1xQcPFG2w=="

            };
            user1.PasswordHash = passwordHasher.HashPassword(user1, "Admin123");
            user2.PasswordHash = passwordHasher.HashPassword(user2, "User123");
            
            builder.Entity<User>().HasData(user1, user2);

            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Aap", Type = ProductType.JUNGLE, Price = 50, Img = "monkey.png" },
                new Product { Id = 2, Name = "Olifant", Type = ProductType.JUNGLE, Price = 50, Img = "elephant.png" },
                new Product { Id = 3, Name = "Zebra", Type = ProductType.JUNGLE, Price = 50, Img = "zebra.png" },
                new Product { Id = 4, Name = "Leeuw", Type = ProductType.JUNGLE, Price = 50, Img = "lion.png" },
                new Product { Id = 5, Name = "Hond", Type = ProductType.FARM, Price = 30, Img = "dog.png" },
                new Product { Id = 6, Name = "Ezel", Type = ProductType.FARM, Price = 30, Img = "donkey.png" },
                new Product { Id = 7, Name = "Koe", Type = ProductType.FARM, Price = 30, Img = "cow.png" },
                new Product { Id = 8, Name = "Eend", Type = ProductType.FARM, Price = 30, Img = "duck.png" },
                new Product { Id = 9, Name = "Kuiken", Type = ProductType.FARM, Price = 30, Img = "chicken.png" },
                new Product { Id = 10, Name = "Pinguïn", Type = ProductType.SNOW, Price = 80, Img = "animal.png" },
                new Product
                {
                    Id = 11, Name = "IJsbeer", Type = ProductType.SNOW, Price = 80, Img = "polar-bear.png"
                },
                new Product { Id = 12, Name = "Zeehond", Type = ProductType.SNOW, Price = 80, Img = "seal.png" },
                new Product { Id = 13, Name = "Kameel", Type = ProductType.DESERT, Price = 100, Img = "camel.png" },
                new Product { Id = 14, Name = "Slang", Type = ProductType.DESERT, Price = 100, Img = "snake.png" },
                new Product { Id = 15, Name = "T-Rex", Type = ProductType.VIP, Price = 150, Img = "dinosaur.png" },
                new Product { Id = 16, Name = "Unicorn", Type = ProductType.VIP, Price = 150, Img = "unicorn.png" }
            );
        }
    }
}
