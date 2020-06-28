using ShoppingCart.Api.Domain.Rules;

namespace ShoppingCart.Api.Domain
{
    public class Category : BaseEntity
    {
        public string Title { get; private set; }
        public Category ParentCategory { get; private set; }

        public Category(string title)
        {
            this.CheckRule(new TitleNotNullOrEmptyRule(title));
            Title = title;
        }

        public Category(string title, Category parentCategory) : this(title)
        {
            this.CheckRule(new TitleNotNullOrEmptyRule(title));
            ParentCategory = parentCategory;
        }
    }
}