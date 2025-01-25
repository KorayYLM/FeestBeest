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

        public bool Add(ProductDto products, int? userId = null)
        {
            var isBasketValid = ChechOrderBasket(userId, products);
            if (!isBasketValid)
            {
                return false;
            }

            basket.Products.Add(products);
            products.InBasket = true;

            return true;
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

        private bool ChechOrderBasket(int? userId = null, ProductDto product = null)
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