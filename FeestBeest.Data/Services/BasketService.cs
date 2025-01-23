using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FeestBeest.Data.Services
{
    public class BasketService
    {
        private readonly Basket basket;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BasketService> logger;

        public BasketService(IServiceProvider serviceProvider, ILogger<BasketService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            basket = new Basket();
        }

        public bool AddToBasket(ProductDto products, int? userId = null)
        {
            logger.LogInformation("Attempting to add product with Id: {ProductId} to basket for user: {UserId}",
                products.Id, userId);

            var isBasketValid = CheckBasket(userId, products);
            if (!isBasketValid)
            {
                logger.LogWarning("CheckBasket failed for product with Id: {ProductId} for user: {UserId}", products.Id,
                    userId);
                return false;
            }

            basket.Products.Add(products);
            products.InBasket = true;

            logger.LogInformation(
                "Product with Id: {ProductId} successfully added to basket for user: {UserId}. IsInBasket: {IsInBasket}",
                products.Id, userId, products.InBasket);
            return true;
        }

        public void RemoveFromBasket(int productId)
        {
            var product = basket.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                basket.Products.Remove(product);
                product.InBasket = false;
                logger.LogInformation("Product with Id: {ProductId} removed from basket", productId);
            }
        }

        public List<ProductDto> GetBasketProducts()
        {
            return basket.Products;
        }

        public void ClearBasket()
        {
            basket.Products.Clear();
        }

        public int GetBasketItemCount()
        {
            return basket.Products.Count;
        }

        private bool CheckBasket(int? userId = null, ProductDto product = null)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var user = userId != null ? userManager.FindByIdAsync(userId.Value.ToString()).Result : null;

                var (checkProducts, _) = new CheckOrderProductsRule().CheckProducts(basket, user, product);
                if (!checkProducts) return false;

                var (checkTogether, _) = new ProductsNotTogetherRule().CheckProductsTogether(basket, product);
                if (!checkTogether) return false;

                var (check, _) = new CheckAnimalAvailabilityRule().CheckAnimalAvailability(basket, product);
                return check;
            }
        }
    }
}