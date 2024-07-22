using Application.Exceptions;
using Application.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case NotFoundException e:
                        context.Response.Redirect("/Error/404");
                        break;
                    case ForbiddenException e:
                        context.Response.Redirect("/Error/403");
                        break;
                    case UnprocessableEntityException e:
                        context.Response.Redirect("/Error/422");
                        break;
                    default:
                        context.Response.Redirect("/Error/500");
                        break;
                }
            }
        }
    }
}
