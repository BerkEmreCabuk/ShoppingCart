using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;

namespace ShoppingCart.Api.Domain.Rules
{
    public class ObjectNotNullRule : IRule
    {
        private readonly object _model;
        private readonly object _modelName;
        internal ObjectNotNullRule(object model, string modelName)
        {
            _model = model;
            _modelName = modelName;
        }
        public string Message => $"{_modelName} is not null.";
        public ExceptionType ExceptionType => ExceptionType.ArgumentNull;

        public bool IsBroken() => _model == null;
    }
}