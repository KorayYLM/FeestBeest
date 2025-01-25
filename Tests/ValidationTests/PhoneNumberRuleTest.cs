using FeestBeest.Data.Rules.ValidationRules;
using NUnit.Framework;

namespace Tests
{
    public class PhoneNumberRuleTest
    {
        private PhoneNumberRule _rule;

        [SetUp]
        public void Setup()
        {
            _rule = new PhoneNumberRule();
        }

        [Test]
        public void IsValid_ValidPhoneNumber_ReturnsTrue()
        {
            // Arrange
            var validPhoneNumber = "1234567890";

            // Act
            var result = _rule.IsValid(validPhoneNumber);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_InvalidPhoneNumber_ReturnsFalse()
        {
            // Arrange
            var invalidPhoneNumber = "12345";

            // Act
            var result = _rule.IsValid(invalidPhoneNumber);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValid_PhoneNumberWithMoreThan15Digits_ReturnsFalse()
        {
            // Arrange
            var longPhoneNumber = "1234567890123456";

            // Act
            var result = _rule.IsValid(longPhoneNumber);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValid_NullValue_ReturnsFalse()
        {
            // Act
            var result = _rule.IsValid(null);

            // Assert
            Assert.IsFalse(result);
        }
    }
}