using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using ShoppingCart.Api.Infrastructures.Exceptions;

namespace ShoppingCart.Api.Infrastructures.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private static readonly EventId _eventId = new EventId(900101, "AplicationException");


        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            logger.LogError(_eventId
                , exception
                , "Url : {@Path} Method: {@Method} Error: {@Message}."
                , UriHelper.GetDisplayUrl(context.Request)
                , context.Request.Method
                , exception.Message);

            context.Response.ContentType = "application/json";
            if (exception is NotificationException notification)
            {
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(notification.Message));
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject("Hata oluştu"));
            }
        }
    }
}