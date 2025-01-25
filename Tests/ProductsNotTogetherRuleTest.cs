using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class ProductsNotTogetherRuleTest
    {
        private ProductsNotTogetherRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new ProductsNotTogetherRule();
        }

        [Test]
        public void CheckProductsTogether_DangerousAnimalWithFarmProduct_ReturnsFalse()
        {
            var basket = new Basket
            {
                Products = new List<ProductDto>
                {
                    new ProductDto { Name = "koe", Type = ProductType.FARM }
                }
            };
            var product = new ProductDto { Name = "leeuw", Type = ProductType.JUNGLE };

            var result = _rule.CheckProductsTogether(basket, product);

            Assert.AreEqual((false, "Nom Nom Nom"), result);
        }

        [Test]
        public void CheckProductsTogether_FarmProductWithDangerousAnimal_ReturnsFalse()
        {
            var basket = new Basket
            {
                Products = new List<ProductDto>
                {
                    new ProductDto { Name = "leeuw", Type = ProductType.JUNGLE }
                }
            };
            var product = new ProductDto { Name = "koe", Type = ProductType.FARM };

            var result = _rule.CheckProductsTogether(basket, product);

            Assert.AreEqual((false, "Nom Nom Nom"), result);
        }

        [Test]
        public void CheckProductsTogether_NoConflict_ReturnsTrue()
        {
            var basket = new Basket
            {
                Products = new List<ProductDto>
                {
                    new ProductDto { Name = "koe", Type = ProductType.FARM }
                }
            };
            var product = new ProductDto { Name = "schaap", Type = ProductType.FARM };

            var result = _rule.CheckProductsTogether(basket, product);

            Assert.AreEqual((true, string.Empty), result);
        }

        [Test]
        public void CheckProductsTogether_EmptyBasket_ReturnsTrue()
        {
            var basket = new Basket
            {
                Products = new List<ProductDto>()
            };
            var product = new ProductDto { Name = "leeuw", Type = ProductType.JUNGLE };

            var result = _rule.CheckProductsTogether(basket, product);

            Assert.AreEqual((true, string.Empty), result);
        }
    }
}