using FeestBeest.Web.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;

namespace Tests
{
    public class OneProductViewModelTest
    {
        private OneProductViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new OneProductViewModel
            {
                Id = 1,
                Name = "Test Product",
                Price = 100,
                Type = ProductType.FARM,
                Img = "test.jpg",
                Check = true,
                Result = "Success",
                Orders = new List<OrderDto>(),
                Products = new List<ProductViewModel>(),
                AvailableImages = new List<string> { "test.jpg" },
                AvailableTypes = new List<ProductType> { ProductType.FARM }
            };
        }

        [Test]
        public void ToDto_ShouldReturnCorrectProductDto()
        {
            // Act
            var dto = _viewModel.ToDto();

            // Assert
            Assert.AreEqual(_viewModel.Id, dto.Id);
            Assert.AreEqual(_viewModel.Name, dto.Name);
            Assert.AreEqual(_viewModel.Price, dto.Price);
            Assert.AreEqual(_viewModel.Type, dto.Type);
            Assert.AreEqual(_viewModel.Img, dto.Img);
        }

        [Test]
        public void Name_ShouldBeRequired()
        {
            // Arrange
            _viewModel.Name = null;

            // Act
            var context = new ValidationContext(_viewModel, null, null) { MemberName = "Name" };
            var result = Validator.TryValidateProperty(_viewModel.Name, context, null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Price_ShouldBeInRange()
        {
            // Arrange
            _viewModel.Price = 0;

            // Act
            var context = new ValidationContext(_viewModel, null, null) { MemberName = "Price" };
            var result = Validator.TryValidateProperty(_viewModel.Price, context, null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Img_ShouldBeRequired()
        {
            // Arrange
            _viewModel.Img = null;

            // Act
            var context = new ValidationContext(_viewModel, null, null) { MemberName = "Img" };
            var result = Validator.TryValidateProperty(_viewModel.Img, context, null);

            // Assert
            Assert.IsFalse(result);
        }
    }
}