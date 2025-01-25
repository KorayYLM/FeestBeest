using FeestBeest.Web.Models;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Tests
{
    public class LoginViewModelTest
    {
        private LoginViewModel _loginViewModel;

        [SetUp]
        public void Setup()
        {
            _loginViewModel = new LoginViewModel
            {
                Email = "john.doe@example.com",
                Password = "password123",
                RememberMe = true
            };
        }

        [Test]
        public void LoginViewModel_ValidModel_DoesNotThrowValidationException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => ValidateModel(_loginViewModel));
        }

        [Test]
        public void LoginViewModel_InvalidEmail_ThrowsValidationException()
        {
            // Arrange
            _loginViewModel.Email = "invalid-email";

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => ValidateModel(_loginViewModel));
            Assert.AreEqual("The Email field is not a valid e-mail address.", ex.Message);
        }

        private void ValidateModel(object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            Validator.ValidateObject(model, validationContext, true);
        }
    }
}