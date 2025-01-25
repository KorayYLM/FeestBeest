using FeestBeest.Data.Dto;
using NUnit.Framework;
using System;

namespace Tests
{
    public class CheckDayOfWeekRuleTest
    {
        private CheckDayOfWeekRule _checkDayOfWeekRule;

        [SetUp]
        public void Setup()
        {
            _checkDayOfWeekRule = new CheckDayOfWeekRule();
        }

        [Test]
        public void IsDayOfWeek_Monday_Returns15()
        {
            // Arrange
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 2) }; // Monday

            // Act
            var result = _checkDayOfWeekRule.IsDayOfWeek(orderDto);

            // Assert
            Assert.AreEqual(15, result);
        }

        [Test]
        public void IsDayOfWeek_Tuesday_Returns15()
        {
            // Arrange
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 3) }; // Tuesday

            // Act
            var result = _checkDayOfWeekRule.IsDayOfWeek(orderDto);

            // Assert
            Assert.AreEqual(15, result);
        }

        [Test]
        public void IsDayOfWeek_Wednesday_Returns0()
        {
            // Arrange
            var orderDto = new OrderDto { OrderFor = new DateOnly(2023, 10, 4) }; // Wednesday

            // Act
            var result = _checkDayOfWeekRule.IsDayOfWeek(orderDto);

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}