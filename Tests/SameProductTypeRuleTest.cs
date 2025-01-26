using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class SameProductTypeRuleTest
    {
        private SameProductTypeRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new SameProductTypeRule();
        }

        [Test]
        public void CheckSameType_ThreeOrMoreSameType_Returns10()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } },
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } },
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } }
                }
            };

            var result = _rule.CheckSameType(orderDto);

            Assert.AreEqual(10, result);
        }

        [Test]
        public void CheckSameType_LessThanThreeSameType_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } },
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } }
                }
            };

            var result = _rule.CheckSameType(orderDto);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckSameType_MixedTypes_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } },
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.JUNGLE } },
                    new OrderDetailsDto { Product = new ProductDto { Type = ProductType.FARM } }
                }
            };

            var result = _rule.CheckSameType(orderDto);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void CheckSameType_EmptyOrder_Returns0()
        {
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>()
            };

            var result = _rule.CheckSameType(orderDto);

            Assert.AreEqual(0, result);
        }
    }
}