using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Rules;

namespace ShoppingCart.Api.Domain
{
    public abstract class Discount : BaseEntity
    {
        public double DiscountAmount { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public Discount(double discountAmount, DiscountType discountType)
        {
            this.CheckRule(new DiscountTypeNotUnknownRule(discountType));
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(discountAmount, "Discount"));
            DiscountAmount = discountAmount;
            DiscountType = discountType;
        }
    }
}