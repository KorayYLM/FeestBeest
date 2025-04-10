﻿using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Data.Rules;
using Moq;

namespace Tests
{
    public class CheckOrderProductsRuleTest
    {
        private CheckOrderProductsRule _rule;
        private Basket _basket;
        private Mock<User> _userMock;

        [SetUp]
        public void SetUp()
        {
            _rule = new CheckOrderProductsRule();
            _basket = new Basket();
            _userMock = new Mock<User>();
        }

        [Test]
        public void CheckProducts_NoUser_TooManyProducts_ReturnsFalse()
        {
            // Arrange
            _basket.Products.AddRange(new List<ProductDto>
            {
                new ProductDto { Type = ProductType.DESERT },
                new ProductDto { Type = ProductType.FARM },
                new ProductDto { Type = ProductType.SNOW }
            });

            // Act
            var result = _rule.CheckProducts(_basket, null);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("You have too many products in your order, only 3 products are allowed and no VIP products.", result.Item2);
        }

        [Test]
        public void CheckProducts_NoUser_VipProduct_ReturnsFalse()
        {
            // Arrange
            var newProduct = new ProductDto { Type = ProductType.VIP };

            // Act
            var result = _rule.CheckProducts(_basket, null, newProduct);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("You have too many products in your order, only 3 products are allowed and no VIP products.", result.Item2);
        }

        [Test]
        public void CheckProducts_SilverUser_TooManyProducts_ReturnsFalse()
        {
            // Arrange
            _userMock.Setup(u => u.Rank).Returns(Rank.SILVER);
            _basket.Products.AddRange(new List<ProductDto>
            {
                new ProductDto { Type = ProductType.DESERT },
                new ProductDto { Type = ProductType.FARM },
                new ProductDto { Type = ProductType.SNOW },
                new ProductDto { Type = ProductType.JUNGLE }
            });

            // Act
            var result = _rule.CheckProducts(_basket, _userMock.Object);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Silver members can only have up to 4 products and no VIP products.", result.Item2);
        }

        [Test]
        public void CheckProducts_GoldUser_VipProduct_ReturnsFalse()
        {
            // Arrange
            _userMock.Setup(u => u.Rank).Returns(Rank.GOLD);
            var newProduct = new ProductDto { Type = ProductType.VIP };

            // Act
            var result = _rule.CheckProducts(_basket, _userMock.Object, newProduct);

            // Assert
            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Only VIP users can add VIP products to the basket.", result.Item2);
        }

        [Test]
        public void CheckProducts_PlatinumUser_VipProduct_ReturnsTrue()
        {
            // Arrange
            _userMock.Setup(u => u.Rank).Returns(Rank.PLATINUM);
            var newProduct = new ProductDto { Type = ProductType.VIP };

            // Act
            var result = _rule.CheckProducts(_basket, _userMock.Object, newProduct);

            // Assert
            Assert.IsTrue(result.Item1);
            Assert.IsEmpty(result.Item2);
        }
    }
}