using OKala_CryptocurrencyDomain.Dtos.ResponseDto;
using System.Net;
using System.Text.Json;

namespace Okala_CryptocurrencyWebApp.Midlewares;

//With a Middleware Class by Convention
public class MyExceptionHandler
{
    private readonly RequestDelegate _next;

    public MyExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            if (ex.Message == "خطا در عملیات")
            {
                var response = new BaseResponseDto<Object>
                {
                    Message = "خطا در عملیات",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                var response = new BaseResponseDto<Object>
                {
                    Message = ex.Message + " * " + ex.StackTrace + " * " + ex.InnerException?.Message,
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            return;
        }

    }
}

public static class MyExceptionHandlerExtensions
{
    public static IApplicationBuilder UseMyExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyExceptionHandler>();
    }
}
