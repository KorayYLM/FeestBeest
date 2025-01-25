using FeestBeest.Data.Dto;
using FeestBeest.Data.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class CheckNameRuleTest
    {
        private CheckNameRule _checkNameRule;

        [SetUp]
        public void Setup()
        {
            _checkNameRule = new CheckNameRule();
        }

        [Test]
        public void CheckForName_ContainsEend_Returns50Or0()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "eend" } }
                }
            };

            // Act
            var result = _checkNameRule.CheckForName(orderDto);

            // Assert
            Assert.IsTrue(result == 50 || result == 0);
        }

        [Test]
        public void CheckForName_DoesNotContainEend_Returns0()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                OrderDetails = new List<OrderDetailsDto>
                {
                    new OrderDetailsDto { Product = new ProductDto { Name = "other" } }
                }
            };

            // Act
            var result = _checkNameRule.CheckForName(orderDto);

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}