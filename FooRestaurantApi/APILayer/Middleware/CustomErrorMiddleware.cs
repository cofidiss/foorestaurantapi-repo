using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Middleware
{
    public class CustomErrorMiddleware
    {
        private readonly ILogger<CustomErrorMiddleware> _logger;
        private readonly RequestDelegate _next;
        public CustomErrorMiddleware(RequestDelegate next, ILogger<CustomErrorMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception message: {ex.Message} - Stack Trace: {ex.StackTrace}");
                context = await CustomExcetionResponce(context, ex);
            }
        }

        private async ValueTask<HttpContext> CustomExcetionResponce(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            var response2 = new ExceptionResponse("There was a server error.", 500);
            var json2 = JsonConvert.SerializeObject(response2);
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync(json2);
            return httpContext;
        }


    }

    public class ExceptionResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ExceptionResponse(string message, int statusCode = 500)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
