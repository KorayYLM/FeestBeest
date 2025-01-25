using FeestBeest.Web.Models;
using FeestBeest.Data.Dto;
using FeestBeest.Data.Models;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Tests
{
    public class ProductViewModelTest
    {
        private ProductViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new ProductViewModel
            {
                Id = 1,
                Name = "Test Product",
                Type = ProductType.FARM,
                Price = 100,
                Img = "test.jpg"
            };
        }

        [Test]
        public void FromDto_ShouldReturnCorrectProductViewModel()
        {
            // Arrange
            var dto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Type = ProductType.FARM,
                Price = 100,
                Img = "test.jpg"
            };

            // Act
            var viewModel = ProductViewModel.FromDto(dto);

            // Assert
            Assert.AreEqual(dto.Id, viewModel.Id);
            Assert.AreEqual(dto.Name, viewModel.Name);
            Assert.AreEqual(dto.Type, viewModel.Type);
            Assert.AreEqual(dto.Price, viewModel.Price);
            Assert.AreEqual(dto.Img, viewModel.Img);
        }

        [Test]
        public void ToDto_ShouldReturnCorrectProductDto()
        {
            // Act
            var dto = _viewModel.ToDto();

            // Assert
            Assert.AreEqual(_viewModel.Id, dto.Id);
            Assert.AreEqual(_viewModel.Name, dto.Name);
            Assert.AreEqual(_viewModel.Type, dto.Type);
            Assert.AreEqual(_viewModel.Price, dto.Price);
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
            _viewModel.Price = -1;

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