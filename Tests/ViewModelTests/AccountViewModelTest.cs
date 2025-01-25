using FeestBeest.Web.Models;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using FeestBeest.Data.Models;

namespace Tests
{
    public class AccountViewModelTest
    {
        private AccountViewModel _accountViewModel;

        [SetUp]
        public void Setup()
        {
            _accountViewModel = new AccountViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Rank = Rank.NONE, 
                HouseNumber = "10A",
                PhoneNumber = "1234567890",
                ZipCode = "12345"
            };
        }

        [Test]
        public void AccountViewModel_ValidModel_DoesNotThrowValidationException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => ValidateModel(_accountViewModel));
        }

        [Test]
        public void AccountViewModel_InvalidEmail_ThrowsValidationException()
        {
            // Arrange
            _accountViewModel.Email = "invalid-email";

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => ValidateModel(_accountViewModel));
            Assert.AreEqual("Invalid email format.", ex.Message);
        }

        private void ValidateModel(object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            Validator.ValidateObject(model, validationContext, true);
        }
    }
}