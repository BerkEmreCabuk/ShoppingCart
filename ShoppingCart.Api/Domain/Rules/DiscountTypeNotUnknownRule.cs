using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;

namespace ShoppingCart.Api.Domain.Rules
{
    public class DiscountTypeNotUnknownRule : IRule
    {
        private readonly DiscountType _type;
        internal DiscountTypeNotUnknownRule(DiscountType type)
        {
            _type = type;
        }
        public string Message => "Discount Type must not be Unknown.";
        public ExceptionType ExceptionType => ExceptionType.Notification;

        public bool IsBroken() => _type == DiscountType.Unknown;
    }
}