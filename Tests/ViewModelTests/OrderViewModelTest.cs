using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using FeestBeest.Web.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class OrderViewModelTest
    {
        private OrderViewModel _orderViewModel;

        [SetUp]
        public void Setup()
        {
            _orderViewModel = new OrderViewModel
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                ZipCode = "12345",
                HouseNumber = "10A",
                PhoneNumber = "1234567890",
                OrderFor = DateOnly.FromDateTime(DateTime.Now),
                Date = DateTime.Now,
                ProductsOverViewModel = new ProductsOverViewModel
                {
                    Products = new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Id = 1, Name = "Product1", Type = ProductType.FARM, Price = 100, Img = "img1.jpg",
                            InBasket = true
                        },
                        new ProductDto
                        {
                            Id = 2, Name = "Product2", Type = ProductType.JUNGLE, Price = 200, Img = "img2.jpg",
                            InBasket = false
                        }
                    }
                },
                Products = new List<Product>(),
                TotalPrice = 300,
                DiscountAmount = 30,
                Result = "Success",
                Check = true
            };
        }

        [Test]
        public void ToDto_ReturnsCorrectOrderDto()
        {
            // Act
            var result = _orderViewModel.ToDto();

            // Assert
            Assert.AreEqual(_orderViewModel.Id, result.Id);
            Assert.AreEqual(_orderViewModel.Name, result.Name);
            Assert.AreEqual(_orderViewModel.Email, result.Email);
            Assert.AreEqual(_orderViewModel.ZipCode, result.ZipCode);
            Assert.AreEqual(_orderViewModel.HouseNumber, result.HouseNumber);
            Assert.AreEqual(_orderViewModel.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(_orderViewModel.OrderFor, result.OrderFor);
            Assert.AreEqual(_orderViewModel.TotalPrice, result.TotalPrice);
            Assert.AreEqual(1, result.OrderDetails.Count);
            Assert.AreEqual(1, result.OrderDetails[0].ProductId);
        }

        [Test]
        public void ToDto_EmptyProducts_ReturnsEmptyOrderDetails()
        {
            // Arrange
            _orderViewModel.ProductsOverViewModel.Products = new List<ProductDto>();

            // Act
            var result = _orderViewModel.ToDto();

            // Assert
            Assert.AreEqual(0, result.OrderDetails.Count);
        }
    }
}