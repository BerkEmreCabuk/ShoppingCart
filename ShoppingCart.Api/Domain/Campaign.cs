using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Rules;

namespace ShoppingCart.Api.Domain
{
    public class Campaign : Discount
    {
        public Category Category { get; private set; }
        public double MinimumQuantity { get; private set; }

        public Campaign(Category category, double minimumQuantity, double discountAmount, DiscountType discountType)
            : base(discountAmount, discountType)
        {
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(minimumQuantity, "Campaign Minimum Quantity"));
            Category = category;
            MinimumQuantity = minimumQuantity;
        }
    }
}