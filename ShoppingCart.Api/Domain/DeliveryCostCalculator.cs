using ShoppingCart.Api.Domain.Interfaces;
using ShoppingCart.Api.Domain.Rules;

namespace ShoppingCart.Api.Domain
{
    public class DeliveryCostCalculator : BaseEntity
    {
        public double CostPerDelivery { get; private set; }
        public double CostPerProduct { get; private set; }
        public double FixedCost { get; private set; }

        public DeliveryCostCalculator(double costPerDelivery, double costPerProduct, double fixedCost = 2.99)
        {
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(costPerDelivery, "Cost Per Delivery"));
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(costPerProduct, "Cost Per Product"));
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(fixedCost, "Fixed Cost"));
            CostPerDelivery = costPerDelivery;
            CostPerProduct = costPerProduct;
            FixedCost = fixedCost;
        }

        public double Calculate(ICart cart)
        {
            this.CheckRule(new ObjectNotNullRule(cart, "Cart"));
            int deliveryCount = cart.GetDeliveryCount();
            int productCount = cart.GetProductCount();
            return (CostPerDelivery * deliveryCount) + (CostPerProduct * productCount) + FixedCost;
        }
    }
}