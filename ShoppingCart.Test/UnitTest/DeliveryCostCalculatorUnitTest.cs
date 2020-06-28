using System;
using Moq;
using ShoppingCart.Api.Domain;
using ShoppingCart.Api.Domain.Interfaces;
using ShoppingCart.Api.Infrastructures.Exceptions;
using Xunit;

namespace ShoppingCart.Test.UnitTest
{
    public class DeliveryCostCalculatorUnitTest
    {
        Mock<ICart> cart;

        public DeliveryCostCalculatorUnitTest()
        {
            cart = new Mock<ICart>();
        }

        #region Calculate
        [Fact]
        public void Calculate_NullCart_ReturnsException()
        {
            var deliveryCostCalculator = new DeliveryCostCalculator(2.5, 3.5);

            Assert.Throws<ArgumentNullException>(() => deliveryCostCalculator.Calculate(null));
        }

        [Fact]
        public void Calculate_EmptyProduct_ReturnsException()
        {
            var deliveryCostCalculator = new DeliveryCostCalculator(2.5, 3.5);
            cart.Setup(x => x.GetDeliveryCount()).Returns(0);
            cart.Setup(x => x.GetProductCount()).Returns(0);
            Assert.Equal(2.99, deliveryCostCalculator.Calculate(cart.Object));
        }
        #endregion
    }
}