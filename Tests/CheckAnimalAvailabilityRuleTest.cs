using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;


namespace Tests
{
    public class CheckAnimalAvailabilityRuleTest
    {
        private CheckAnimalAvailabilityRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new CheckAnimalAvailabilityRule();
        }

        [Test]
        public void CheckAnimalAvailability_PenguinOnWeekend_ReturnsFalse()
        {
            var basket = new Basket { Products = new List<ProductDto>() };
            var product = new ProductDto { Name = "Pinguïn", Type = ProductType.SNOW };

            var result = _rule.CheckAnimalAvailability(basket, product);

            Assert.AreEqual((false, "This product is only available on weekdays"), result);
        }

        [Test]
        public void CheckAnimalAvailability_DesertProductInWinter_ReturnsFalse()
        {
            var basket = new Basket { Products = new List<ProductDto>() };
            var product = new ProductDto { Type = ProductType.DESERT };

            var result = _rule.CheckAnimalAvailability(basket, product);

            Assert.AreEqual((false, "Its to cold right now for this product"), result);
        }

        [Test]
        public void CheckAnimalAvailability_SnowProductInSummer_ReturnsFalse()
        {
            var basket = new Basket { Products = new List<ProductDto>() };
            var product = new ProductDto { Type = ProductType.SNOW };

            var result = _rule.CheckAnimalAvailability(basket, product);

            Assert.AreEqual((false, "Metlinggg"), result);
        }

        [Test]
        public void CheckAnimalAvailability_NoConflict_ReturnsTrue()
        {
            var basket = new Basket { Products = new List<ProductDto>() };
            var product = new ProductDto { Name = "Koe", Type = ProductType.FARM };

            var result = _rule.CheckAnimalAvailability(basket, product);

            Assert.AreEqual((true, string.Empty), result);
        }

        [Test]
        public void CheckAnimalAvailability_PenguinInBasketOnWeekend_ReturnsFalse()
        {
            var basket = new Basket
            {
                Products = new List<ProductDto>
                {
                    new ProductDto { Name = "Pinguïn", Type = ProductType.SNOW }
                }
            };
            var product = new ProductDto { Name = "Koe", Type = ProductType.FARM };

            var result = _rule.CheckAnimalAvailability(basket, product);

            Assert.AreEqual((false, "This product is only available on weekdays"), result);
        }
    }
}