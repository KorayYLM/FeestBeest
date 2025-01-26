using FeestBeest.Data.Dto;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class NameContainsRuleTest
    {
        private NameContainsRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new NameContainsRule();
        }

        [Test]
        public void ApplyNameContainsDiscount_UniqueLetters_ReturnsCorrectDiscount()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "Apple" } },
                    new OrderDetailsDto { Product = new ProductDto { Name = "Banana" } }
                }
            };

            var result = _rule.ApplyNameContainsDiscount(orderDto);

            Assert.AreEqual(10, result); // Unique letters: a, p, l, e, b, n -> 5 unique letters * 2 = 10
        }

        [Test]
        public void ApplyNameContainsDiscount_NoProducts_ReturnsZero()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>()
            };

            var result = _rule.ApplyNameContainsDiscount(orderDto);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void ApplyNameContainsDiscount_SameLettersInProducts_ReturnsCorrectDiscount()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "Apple" } },
                    new OrderDetailsDto { Product = new ProductDto { Name = "Pineapple" } }
                }
            };

            var result = _rule.ApplyNameContainsDiscount(orderDto); 

            Assert.AreEqual(12, result); // Unique letters: a, p, l, e, i, n -> 6 unique letters * 2 = 12
        }
    }
}