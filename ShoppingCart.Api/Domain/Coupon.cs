using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Rules;

namespace ShoppingCart.Api.Domain
{
    public class Coupon : Discount
    {
        public double MinimumAmount { get; private set; }
        public Coupon(double minimumAmount, double discountAmount, DiscountType discountType)
            : base(discountAmount, discountType)
        {
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(minimumAmount, "Coupon Minimum Amount"));
            MinimumAmount = minimumAmount;
        }
    }
}