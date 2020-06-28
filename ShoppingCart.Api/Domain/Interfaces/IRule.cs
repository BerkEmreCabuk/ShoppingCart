using ShoppingCart.Api.Domain.Enums;

namespace ShoppingCart.Api.Domain.Interfaces
{
    public interface IRule
    {
        bool IsBroken();

        string Message { get; }
        ExceptionType ExceptionType { get; }
    }
}