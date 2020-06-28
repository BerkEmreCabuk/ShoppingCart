using ShoppingCart.Api.Domain.Rules;

namespace ShoppingCart.Api.Domain
{
    public class Product : BaseEntity
    {
        public string Title { get; private set; }
        public double Price { get; private set; }
        public Category Category { get; private set; }

        public Product(string title, double price, Category category)
        {
            this.CheckRule(new TitleNotNullOrEmptyRule(title));
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(price, "Price"));
            Title = title;
            Price = price;
            Category = category;
        }
    }
}