using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;

namespace ShoppingCart.Api.Domain.Rules
{
    public class TitleNotNullOrEmptyRule : IRule
    {
        private readonly string _title;
        internal TitleNotNullOrEmptyRule(string title)
        {
            _title = title;
        }
        public string Message => "Title is not null or empty";
        public ExceptionType ExceptionType => ExceptionType.Notification;

        public bool IsBroken() => string.IsNullOrEmpty(_title);
    }
}