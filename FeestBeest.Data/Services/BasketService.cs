using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FeestBeest.Data.Services
{
    public class BasketService
    {
        private readonly Basket basket;
        private readonly IServiceProvider serviceProvider;

        public BasketService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            basket = new Basket();
        }

        public (bool, string) Add(ProductDto product, int? userId = null)
        {
            var (isValid, message) = CheckOrderBasket(userId, product);
            if (!isValid)
            {
                return (false, message);
            }

            basket.Products.Add(product);
            product.InBasket = true;

            return (true, "Product added successfully");
        }

        public void Remove(int productId)
        {
            var product = basket.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                basket.Products.Remove(product);
                product.InBasket = false;
            }
        }

        public List<ProductDto> GetBasketProducts()
        {
            return basket.Products;
        }

        public void Clear()
        {
            basket.Products.Clear();
        }

        public int GetBasketCount()
        {
            return basket.Products.Count;
        }

        private (bool, string) CheckOrderBasket(int? userId, ProductDto product)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var user = userId.HasValue ? userManager.FindByIdAsync(userId.Value.ToString()).Result : null;

                var (checkProducts, productsMessage) = new CheckOrderProductsRule().CheckProducts(basket, user, product);
                if (!checkProducts) return (false, productsMessage);

                var (checkTogether, togetherMessage) = new ProductsNotTogetherRule().CheckProductsTogether(basket, product);
                if (!checkTogether) return (false, togetherMessage);

                var (checkAvailability, availabilityMessage) = new CheckAnimalAvailabilityRule().CheckAnimalAvailability(basket, product);
                return !checkAvailability ? (false, availabilityMessage) : (true, string.Empty);
            }
        }
    }
}