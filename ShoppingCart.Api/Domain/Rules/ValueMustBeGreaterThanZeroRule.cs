using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;

namespace ShoppingCart.Api.Domain.Rules
{
    public class ValueMustBeGreaterThanZeroRule : IRule
    {
        private readonly double _value;
        private readonly string _valueName;
        internal ValueMustBeGreaterThanZeroRule(double value, string valueName)
        {
            _value = value;
            _valueName = valueName;
        }
        public string Message => $"{_valueName} must be greater zero.";
        public ExceptionType ExceptionType => ExceptionType.Notification;

        public bool IsBroken() => _value <= 0;
    }
}