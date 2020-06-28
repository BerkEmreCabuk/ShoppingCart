using System;
using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;
using ShoppingCart.Api.Infrastructures.Exceptions;

namespace ShoppingCart.Api.Domain
{
    public abstract class BaseEntity
    {
        protected void CheckRule(IRule rule)
        {
            if (rule.IsBroken())
            {
                switch (rule.ExceptionType)
                {
                    case ExceptionType.ArgumentNull:
                        throw new ArgumentNullException(rule.Message);
                    case ExceptionType.Notification:
                    default:
                        throw new NotificationException(rule.Message);
                }

            }
        }
    }
}